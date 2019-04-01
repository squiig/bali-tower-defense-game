using Game.Entities.EventContainers;
using Game.Entities.Interfaces;
using UnityEngine;

namespace Game.Entities.Towers
{
	public abstract class TowerBase : Entity, IAggressor
	{
		private SphereCollider _SphereCollider;
		private float _attackCoolDown = 0.0f;
		private const float ATTACK_COOL_DOWN_DURATION = 3.0f;

		[SerializeField] protected float StartAttackRange, MaxAttackRange, AttackRange;
		[SerializeField] protected TowerAttack Attack;
		[SerializeField] protected IDamageable TargetDamageable;

		[SerializeField] private Allegiance _allegiance;
		[SerializeField] private bool _isDebug = false;

		public float GetRange() => AttackRange;

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

			Debug.DrawLine(GetLocation(), TargetDamageable.GetEntity().transform.position, Color.red);
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
			UnlinkEnemy();
		}

		private void UnlinkEnemy()
		{
			if (TargetDamageable == null)
				return;

			TargetDamageable.OnDeath -= OnTargetDeath;
			TargetDamageable = null;
		}

		private bool ShouldAttack() => _attackCoolDown <= 0 && TargetDamageable != null && !IsTargetForsaken();

		/// <inheritdoc />
		/// <summary>
		/// Used to forcefully attack the current target.
		/// If this instance has no target, target is out of
		/// range, or is reloading. This instance will stand idle.
		/// </summary>
		public void ExecuteAttack()
		{
			if (!ShouldAttack())
				return;

			_attackCoolDown = ATTACK_COOL_DOWN_DURATION;
			Attack.ExecuteAttack(TargetDamageable, TargetDamageable.GetEntity().GetLocation());
		}

		private bool IsTargetForsaken()
		{
			if (Vector3.Distance(GetLocation(), TargetDamageable.GetEntity().GetLocation()) < AttackRange
				|| TargetDamageable.GetEntity().isActiveAndEnabled)
				return false;

			TargetDamageable = null;
			return true;
		}

		private void OnTriggerExit(Collider other)
		{
			IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

			if (damageable != null && TargetDamageable != null && TargetDamageable == damageable)
				UnlinkEnemy();
		}

		private void OnTriggerStay(Collider other)
		{
			IDamageable damageable;

			if ((damageable = other.gameObject.GetComponent<IDamageable>()) != null &&
			    TargetDamageable != null && damageable.GetPriority() > TargetDamageable.GetPriority())
				return;

			TargetDamageable = damageable;
			TargetDamageable.OnDeath += OnTargetDeath;
		}
	}
}
