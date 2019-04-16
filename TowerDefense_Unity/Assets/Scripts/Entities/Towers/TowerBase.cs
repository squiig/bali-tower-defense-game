using System.Collections.Generic;
using System.Linq;
using Game.Entities.EventContainers;
using Game.Entities.Interfaces;
using Game.Entities.MovingEntities;
using UnityEngine;

namespace Game.Entities.Towers
{
	public abstract class TowerBase : Entity, IAggressor
	{
		private SphereCollider _SphereCollider;
		private float _attackCoolDown = 0.0f;
		private const float ATTACK_COOL_DOWN_DURATION = 3.0f;

		[SerializeField] protected float StartAttackRange, MaxAttackRange, AttackRange, TowerPrice;
		[SerializeField] protected TowerAttack Attack;
		[SerializeField] protected IDamageable TargetDamageable;

		private Allegiance _allegiance = Allegiance.FRIENDLY;
		[SerializeField] private bool _isDebug = false;

		public float GetRange() => AttackRange;
		public float GetPrice() => TowerPrice;

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
			Debug.Log($"Tower [{GetHashCode()}]: Target has died.");
			sender.OnDeath -= OnTargetDeath;
			TargetDamageable = null;

			FindNewTarget();
		}

		private void FindNewTarget()
		{
			RaycastHit[] targetsHit = Physics.SphereCastAll(transform.position, AttackRange, Vector3.forward);

			if (targetsHit.Length < 1)
				return;

			List<IDamageable> possibleTargets = new List<IDamageable>();

			for (int i = 0; i < targetsHit.Length; i++)
			{
				IDamageable damageable;
				if((damageable = targetsHit[i].transform.GetComponent<IDamageable>()) == null)
					continue;

				possibleTargets.Add(damageable);
			}

			TargetDamageable = possibleTargets.FirstOrDefault(x =>
				x.GetAllegiance() != _allegiance);


			Debug.Log(string.Format("name: [{0}], allegiance [{1}] because we are {2}",
				TargetDamageable == null ? "no target" : TargetDamageable.GetEntity().name,
				TargetDamageable == null ? "no target" : TargetDamageable.GetAllegiance().ToString(), _allegiance));
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

			if(Attack.GetAttackType() == AttackType.SINGLE_TARGET)
				RangedAttack();
			else
				AreaAttack();
		}

		private void AreaAttack()
		{
			Attack.ExecuteAttack(TargetDamageable, TargetDamageable.GetEntity().GetLocation());
		}

		private void RangedAttack()
		{
			ProjectilePool.Instance.ActivateObject(x => x != null)?.
				InitializeAndActivate(transform.position, TargetDamageable, Attack);
		}

		private bool IsTargetForsaken()
		{
			if (Vector3.Distance(GetLocation(), TargetDamageable.GetEntity().GetLocation()) < AttackRange)
				return false;

			Debug.Log("Target Forsaken!");
			TargetDamageable = null;
			FindNewTarget();
			return true;
		}

		private void OnTriggerEnter(Collider other)
		{
			if(_isDebug)
				Debug.Log($"Tower {GetHashCode()}: Found damageable [{other.gameObject.GetComponent<IDamageable>() != null}]");

			if (other.gameObject.GetComponent<IDamageable>() == null)
				return;

			if (TargetDamageable == null)
			{
				FindNewTarget();
				return;
			}

			IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
			if (damageable.GetPriority() <= TargetDamageable.GetPriority() || damageable.GetAllegiance() == _allegiance)
				return;

			Debug.Log($"Better target found going from {TargetDamageable.GetPriority()} to {damageable.GetPriority()}");
			TargetDamageable = damageable;

			if (TargetDamageable == null || !_isDebug)
				return;

			TargetDamageable.OnDeath += OnTargetDeath;
		}
	}
}
