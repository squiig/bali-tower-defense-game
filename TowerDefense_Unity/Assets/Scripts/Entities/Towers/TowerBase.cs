using System.Linq;
using Game.Entities.EventContainers;
using Game.Entities.Interfaces;
using Game.Entities.MovingEntities;
using UnityEngine;

namespace Game.Entities.Towers
{
	public abstract class TowerBase : Entity, IAggressor
	{
		private float _attackCoolDown = 0.0f;
		private const float ATTACK_COOL_DOWN_DURATION = 3.0f;

		[SerializeField] protected readonly float StartAttackRange;
		[SerializeField] protected readonly float MaxAttackRange;
		[SerializeField] protected float AttackRange;
		[Space]
		[SerializeField] protected TowerAttack Attack;
		[Space]
		[SerializeField] protected IDamageable TargetDamageable;
		[Space]
		[SerializeField] private Allegiance _allegiance;
		[Space]
		[SerializeField] private bool _isDebug = false;

		private SphereCollider _SphereCollider;

		/// <inheritdoc />
		/// <summary>
		/// Used to get the attack of this IAggressor.
		/// Returns the attack class of this instance.
		/// </summary>
		/// <returns>Returns the attack class of this instance.</returns>
		public IAttack GetAttack() => Attack;

		/// <inheritdoc />
		/// <summary>
		/// Forcefully override the target of this entity.
		/// Target is set by the aggressor itself, but can
		/// be forcefully overriden at will.
		/// If target is out of range, this will reset to attack targets in range.
		/// </summary>
		/// <param name="target"> Target to focus upon.</param>
		public void SetTarget(in IDamageable target) => TargetDamageable = target;

		private void Awake()
		{
			_SphereCollider = gameObject.AddComponent<SphereCollider>();
			_SphereCollider.radius = MaxAttackRange;
			_SphereCollider.isTrigger = true;
		}

		private void Update()
		{
			if (_isDebug)
				DrawDebug();

			TargetingAndAttacks();
		}

		private void DrawDebug()
		{
			if (TargetDamageable == null)
				return;

			Debug.DrawLine(GetLocation(), TargetDamageable.GetPosition(), Color.red);
		}

		private void OnDrawGizmos()
		{
			if(!_isDebug)
				return;

			Gizmos.DrawWireSphere(GetLocation(), AttackRange);

			if (TargetDamageable == null)
				return;

			Gizmos.DrawWireSphere(GetLocation(), 1.0f);
		}

		private void TargetingAndAttacks()
		{
			if (_attackCoolDown > 0)
				_attackCoolDown -= Time.unscaledDeltaTime;

			ExecuteAttack();
		}

		private void OnTargetDeath(in IDamageable sender, in EntityDamaged payload)
		{
			Debug.Log($"Tower [{GetHashCode()}]: Target has died.");
			sender.OnDeath -= OnTargetDeath;
			TargetDamageable = null;
		}

		/// <inheritdoc />
		/// <summary>
		/// Used to forcefully attack the current target.
		/// If this instance has no target, target is out of
		/// range, or is reloading. This instance will stand idle.
		/// </summary>
		public void ExecuteAttack()
		{
			if (_attackCoolDown > 0 || TargetDamageable == null)
				return;

			if (IsTargetForsaken())
				return;

			_attackCoolDown = ATTACK_COOL_DOWN_DURATION;

			if (Attack.GetAttackType() == AttackType.SINGLE_TARGET)
			{
				Attack.ExecuteAttack(TargetDamageable, GetLocation());
				return;
			}

			Attack.ExecuteAttack(null, GetLocation());
		}

		private bool IsTargetForsaken()
		{
			if (Vector3.Distance(GetLocation(), TargetDamageable.GetPosition()) < AttackRange)
				return false;

			TargetDamageable = null;
			return true;
		}

		private void OnTriggerEnter(Collider collider)
		{
			if(_isDebug)
				Debug.Log($"Tower {GetHashCode()}: Found damageable [{collider.gameObject.GetComponent<IDamageable>() != null}]");

			IDamageable damageable;

			if ((damageable = collider.gameObject.GetComponent<IDamageable>()) != null &&
			    TargetDamageable != null && damageable.GetPriority() > TargetDamageable.GetPriority())
				return;

			TargetDamageable = damageable;

			if (TargetDamageable == null || !_isDebug)
				return;

			TargetDamageable.OnDeath += OnTargetDeath;
		}
	}
}
