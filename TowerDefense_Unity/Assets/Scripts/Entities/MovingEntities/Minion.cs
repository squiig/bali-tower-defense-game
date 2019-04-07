using Game.Entities.EventContainers;
using Game.Entities.Interfaces;
using UnityEngine;

namespace Game.Entities.MovingEntities
{
	public class Minion : TraversingEntity, IPoolable
	{
		private float _AttackCoolDown = 0.0f;
		private const float ATTACK_COOL_DOWN_DURATION = 3.0f;
		private SphereCollider _SphereCollider;

		[SerializeField] private float _MaxHealth, _MaxRange;
		[SerializeField] private int _AttackPriority;
		[SerializeField] private MinionAttack _MinionAttack;
		[SerializeField] private Allegiance _Allegiance;
		[SerializeField] private bool _IsDebug = false;

		private void Awake()
		{
			Initialize(_MaxHealth, _AttackPriority, _Allegiance, _MinionAttack);

			SetObjectDependencies();
			base.OnDeath += OnDeath;
		}

		private new void OnDeath(in IDamageable sender, in EntityDamaged payload)
		{
			ReleaseOwnership();
		}

		private void SetObjectDependencies()
		{
			_SphereCollider = gameObject.AddComponent<SphereCollider>();
			_SphereCollider.radius = _MaxRange;
			_SphereCollider.isTrigger = true;

			Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
			rigidbody.useGravity = false;
			rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		}

		protected override void Update()
		{
			base.Update();
			
            if (_IsDebug)
                DrawDebug();

			AttackAndCooldown();
		}

        private void AttackAndCooldown()
        {
	        if (_AttackCoolDown > 0)
                _AttackCoolDown -= Time.deltaTime;

			ExecuteAttack();
		}

		private void DrawDebug()
		{
			if (TargetIDamageable == null)
				return;

            Debug.DrawLine(GetPosition(), TargetIDamageable.GetEntity().GetLocation(), Color.red);
        }

		private void OnDrawGizmos()
		{
			if(!_IsDebug || TargetIDamageable == null)
				return;

			Gizmos.DrawWireSphere(GetPosition(), 1.0f);
        }

        /// <inheritdoc />
        /// <summary>
        /// Used to forcefully attack the current target.
        /// If this instance has no target, target is out of
        /// range, or is reloading. This instance will stand idle.
        /// </summary>
        public override void ExecuteAttack()
		{			
			if (_AttackCoolDown > 0 || TargetIDamageable == null)
				return;

			if (IsTargetForsaken())
				return;

			_AttackCoolDown = ATTACK_COOL_DOWN_DURATION;

			if (Attack != null)
			{
				Attack.ExecuteAttack(TargetIDamageable);
			}
			else
			{
				Debug.LogWarning("Tried attacking, but attack is null.");
			}

			ReleaseOwnership();
		}

		private bool IsTargetForsaken()
		{
			return false;
			//if (Vector3.Distance(GetPosition(), TargetIDamageable.GetEntity().GetLocation()) < _MaxRange)
			//	return false;
			//
			//TargetIDamageable = null;
			//return true;
        }

		private void OnTriggerEnter(Collider other)
		{
			if(_IsDebug)
				Debug.Log($"Minion {GetHashCode()} Found damageable [{other.gameObject.GetComponent<IDamageable>() != null}]");

			IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

			if (damageable == null)
				return;

			if (TargetIDamageable != null)
			{
				if(damageable.GetPriority() < TargetIDamageable.GetPriority())
					return;
			}

			if (damageable.GetAllegiance() == GetAllegiance())
				return;

			TargetIDamageable = damageable;
		}

		public void Kill()
		{
			ReleaseOwnership();
		}
	}
}
