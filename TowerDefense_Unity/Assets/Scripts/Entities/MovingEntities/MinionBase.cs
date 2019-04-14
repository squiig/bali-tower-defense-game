using System;
using System.Collections;
using System.Linq;
using Game.Entities.EventContainers;
using Game.Entities.Interfaces;
using Game.SplineSystem;
using Game.Utils;
using UnityEngine;

namespace Game.Entities.MovingEntities
{
	public abstract class MinionBase : Entity, IDamageable, IAggressor
	{
		[SerializeField] private const float UPDATE_NEXT_DESTINATION = 0.1f;
		[SerializeField] protected float _MoveSpeed = 7.0f;
		[SerializeField] protected float _RotationSpeed = 5.0f;

		private float _StartHealth;
		private int _Priority;
		private bool _IsConducting;
		private int _CurrentDestinationIndex;
		private SplinePathManager _PathManager;
		private readonly WaitForSeconds _SlowDuration = new WaitForSeconds(2);

		protected IAttack Attack;
		protected Allegiance Allegiance;
		protected IDamageable TargetIDamageable;
		protected float Health;


		public BezierSplineDataObject SplineBranch { get; set; }
		/// <inheritdoc />
		/// <summary>
		/// Event handler that is fired when this instance is hit.
		/// Gives both this instance and the event class <see cref="T:Game.Entities.EventContainers.EntityDamaged" />
		/// </summary>
		public event TypedEventHandler<IDamageable, EntityDamaged> OnHit;
		/// <inheritdoc />
		/// <summary>
		/// Event handler that is fired when this instance is hit.
		/// Gives both this instance and the event class <see cref="T:Game.Entities.EventContainers.EntityDamaged" />
		/// </summary>
		public event TypedEventHandler<IDamageable, EntityDamaged> OnDeath;

		protected virtual void Start()
		{
			if (FindObjectOfType<SplinePathManager>() != null)
				_PathManager = FindObjectOfType<SplinePathManager>();
		}

		protected void Initialize(float maxHealth, int priority, Allegiance allegiance, in IAttack attack)
		{
			Allegiance = allegiance;
            Health = maxHealth;
            _StartHealth = maxHealth;
			_Priority = priority;
			Attack = attack;
		}

		protected virtual void Update()
		{
			if (_PathManager == null || !_IsConducting)
				return;

			Vector3 delta = _PathManager[SplineBranch, _CurrentDestinationIndex] - transform.position;
			if (new Vector3(delta.x, 0, delta.z).magnitude < UPDATE_NEXT_DESTINATION)
				UpdateSplineDestinationPoint();
			else
				MoveSplineWalker();
		}

		private void MoveSplineWalker()
		{
			if (_PathManager.EvenlySpacedSplinePointCount(SplineBranch) - 1 >= _CurrentDestinationIndex + 1)
			{
				transform.rotation = Quaternion.Slerp(
					a: transform.rotation,
					b: Quaternion.LookRotation(Bezier3DUtility.GetTangent(
						_PathManager[SplineBranch, _CurrentDestinationIndex + 1],
						_PathManager[SplineBranch, _CurrentDestinationIndex])),
					t: _RotationSpeed * Time.deltaTime);
			}

			transform.position = Vector3.MoveTowards(
				current: transform.position,
				target: new Vector3(
					x: _PathManager[SplineBranch, _CurrentDestinationIndex].x,
					y: transform.position.y,
					z: _PathManager[SplineBranch, _CurrentDestinationIndex].z),
				maxDistanceDelta: _MoveSpeed * Time.deltaTime);
		}

		private void UpdateSplineDestinationPoint()
		{
			if (_PathManager.EvenlySpacedSplinePointCount(SplineBranch) - 1 < _CurrentDestinationIndex + 1)
				return;
			_CurrentDestinationIndex++;
		}

		/// <inheritdoc />
		/// <summary>
		/// Returns the position of this unit.
		/// </summary>
		/// <returns>Returns the position of this unit.</returns>
		public Vector3 GetPosition() => GetLocation();

		/// <inheritdoc />
		/// <summary>
		/// Used to get the priority of this damageable.
		/// Targets with lower priority will be targeted last.
		/// </summary>
		/// <returns> The priority to attack this instance of this damageable.</returns>
		public int GetPriority() => _Priority;

		/// <inheritdoc />
		/// <summary>
		/// Used to check whether this object is currently in the game
		/// or is only available in the pool.
		/// [TRUE] if in scene, [FALSE] if only available from pool.
		/// </summary>
		/// <returns> [TRUE] if in scene, [FALSE] if only available from pool.</returns>
		public bool IsConducting() => _IsConducting;

		/// <inheritdoc />
		/// <summary>
		/// Activate this instance. Called when an object has been requested from
		/// the pool.
		/// </summary>
        public void Activate()
		{
			_IsConducting = true;
			SetActive(true);

			_CurrentDestinationIndex = 0;
			Health = _StartHealth;
			OnHit?.Invoke(this, new EntityDamaged(this, _StartHealth, _StartHealth));
		}

		/// <inheritdoc />
		/// <summary>
		/// Releases ownership of this object and returns it to the pool.
		/// </summary>
        public void ReleaseOwnership()
        {
	        SetActive(false);
            _IsConducting = false;
        }

		/// <inheritdoc />
		/// <summary>
		/// Returns the allegiance of this unit
		/// </summary>
		/// <returns></returns>
		public Allegiance GetAllegiance() => Allegiance;

        /// <inheritdoc />
        /// <summary>
        /// Returns the GameObject of this instance.
        /// </summary>
        /// <returns>Returns the GameObject of this instance.</returns>
        public Entity GetEntity() => this;

        /// <inheritdoc />
		/// <summary>
		/// Used when this instance gets hit.
		/// Will apply the contents of the <see cref="T:Game.Entities.OnHitEffects" />
		/// Will fire the events.
		/// </summary>
		/// <param name="onHitEffects"> Class containing data about the attack</param>
		public void ApplyOnHitEffects(in OnHitEffects onHitEffects)
		{
			float previousHealth = Health;
			Health -= onHitEffects.GetDamage();

			EntityDamaged payload = new EntityDamaged(this, Health, previousHealth);
			
			OnHit?.Invoke(this, payload);

			if(Health <= 0)
				OnDeath?.Invoke(this, payload);

			if (onHitEffects.GetStatusEffects().Any(x => x == StatusEffects.SLOWED))
				StartCoroutine(MovementImpaired(StatusEffects.SLOWED, 60));
		}

        /// <inheritdoc />
		/// <summary>
		/// Used to get the current health of this instance
		/// When first spawned will always indicate max health.
		/// </summary>
		/// <returns> The current health of this instance. </returns>
		public  float GetHealth() => Health;

		/// <inheritdoc />
		/// <summary>
		/// Forcefully override the target of this entity.
		/// Target is set by the aggressor itself, but can
		/// be forcefully overriden at will.
		/// If target is out of range, this will reset to attack targets in range.
		/// </summary>
		/// <param name="target"> Target to focus upon.</param>
		public void SetTarget(in IDamageable target) => TargetIDamageable = target; 

		/// <inheritdoc />
		/// <summary>
		/// Used to forcefully attack the current target.
		/// If this instance has no target, target is out of
		/// range, or is reloading. This instance will stand idle.
		/// </summary>
		public abstract void ExecuteAttack();

		/// <inheritdoc />
		/// <summary>
		/// Used to get the attack of this IAggressor.
		/// Returns the attack class of this instance.
		/// </summary>
		/// <returns>Returns the attack class of this instance.</returns>
		public IAttack GetAttack() => Attack;

		private IEnumerator MovementImpaired(StatusEffects effect, float percent)
		{
			float previousSpeed = _MoveSpeed;

			if (effect == StatusEffects.NONE)
				yield break;

			_MoveSpeed = (_MoveSpeed / 100.0f) * percent;

			yield return _SlowDuration;
			_MoveSpeed = previousSpeed;
		}
	}
}
