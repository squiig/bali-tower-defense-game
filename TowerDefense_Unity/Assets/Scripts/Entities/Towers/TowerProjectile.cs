using Game.Entities.Interfaces;
using UnityEngine;

namespace Game.Entities.Towers
{
	public class TowerProjectile : MonoBehaviour , IPoolable
	{
		private IDamageable _Target;

		private IAttack _Attack;

		private Vector3 _StartPosition;
		private Vector3 _LastKnownPosition;

		[SerializeField] private float _Speed = 15.0f;
		[SerializeField] private float _DamageInflictingThreshold = 1.0f;

		private bool _IsConducting = false;

		/// <inheritdoc />
		/// <summary>
		/// Used to check whether this object is currently in the game
		/// or is only available in the pool.
		/// [TRUE] if in scene, [FALSE] if only available from pool.
		/// </summary>
		/// <returns> [TRUE] if in scene, [FALSE] if only available from pool.</returns>
		public bool IsConducting() => _IsConducting;

		public void InitializeAndActivate(Vector3 position, IDamageable target, IAttack attack)
		{
			_StartPosition = position;
			_Target = target;
			_Attack = attack;

			target.OnDeath += TargetOnDeath;

			Activate();
		}

		private void TargetOnDeath(in IDamageable sender, in EventContainers.EntityDamaged payload)
		{
			sender.OnDeath -= TargetOnDeath;

			_LastKnownPosition = sender.GetEntity().GetLocation();

			_Target = null;
		}

		/// <inheritdoc />
		/// <summary>
		/// Activate this instance. Called when an object has been requested from
		/// the pool.
		/// </summary>
		public void Activate()
		{
			if (_Target == null || _Attack == null)
			{
				Debug.LogError("Initialize the projectile with the parameters before activating it");
				return;
			}

			_IsConducting = true;
			transform.position = _StartPosition;
			gameObject.SetActive(true);
		}

		private void Update() => GoToTarget(_Target?.GetEntity().GetLocation() ?? _LastKnownPosition);

		private void GoToTarget(Vector3 targetLocation)
		{
			if (!_IsConducting)
				return;

			transform.position = Vector3.MoveTowards(transform.position, targetLocation, _Speed * Time.deltaTime);
			transform.LookAt(targetLocation);


			if (Vector3.Distance(transform.position, targetLocation) > _DamageInflictingThreshold)
				return;

			TargetHit();
		}

		private void TargetHit()
		{
			if (_Target != null)
				_Attack.ExecuteAttack(_Target, null);

			ReleaseOwnership();
		}

		/// <inheritdoc />
		/// <summary>
		/// Releases ownership of this object and returns it to the pool.
		/// </summary>
		public void ReleaseOwnership()
		{
			_IsConducting = false;

			transform.position = Vector3.zero;
			gameObject.SetActive(false);

			_Target = null;
			_Attack = null;
		}
	}
}
