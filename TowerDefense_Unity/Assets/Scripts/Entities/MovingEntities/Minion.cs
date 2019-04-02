using System.Linq;
using Game.Entities.EventContainers;
using Game.Entities.Interfaces;
using UnityEngine;


namespace Game.Entities.MovingEntities
{
	public class Minion : TraversingEntity
	{
		private float _attackCoolDown = 0.0f;
		private const float ATTACK_COOL_DOWN_DURATION = 3.0f;

		[SerializeField] private float _maxHealth;
		[SerializeField] private float _maxRange;
		[SerializeField] private int _attackPriority;
		[Space]
		[SerializeField] private MinionAttack _MinionAttack;
		[Space]
		[SerializeField] private Allegiance _allegiance;
		[Space]
		[SerializeField] private bool _isDebug = false;

		private SphereCollider _SphereCollider;

		private void Awake()
		{
			Initialize(_maxHealth, _attackPriority, _allegiance, _MinionAttack);

			SetObjectDependencies();
			base.OnDeath += OnDeath;
		}

		private void OnDeath(in IDamageable sender, in EntityDamaged payload)
		{
			ReleaseOwnership();
		}

		private void SetObjectDependencies()
		{
			_SphereCollider = gameObject.AddComponent<SphereCollider>();
			_SphereCollider.radius = _maxRange;
			_SphereCollider.isTrigger = true;

			gameObject.AddComponent<Rigidbody>().useGravity = false;
		}

		protected override void Update()
		{
			base.Update();
			
            if (_isDebug)
                DrawDebug();

            AttackAndCooldown();
        }

        private void AttackAndCooldown()
        {
	        if (_attackCoolDown > 0)
                _attackCoolDown -= Time.deltaTime;

            ExecuteAttack();
        }

        private void DrawDebug()
        {
			if (TargetIDamageable == null)
				return;

            Debug.DrawLine(GetPosition(), TargetIDamageable.GetPosition(), Color.red);
        }

		private void OnDrawGizmos()
		{
			if(!_isDebug || TargetIDamageable == null)
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
			if (_attackCoolDown > 0 || TargetIDamageable == null)
				return;

			if(IsTargetForsaken())
				return;

			_attackCoolDown = ATTACK_COOL_DOWN_DURATION;
			Attack.ExecuteAttack(TargetIDamageable, GetLocation());

			ReleaseOwnership();
		}

		private bool IsTargetForsaken()
		{
			if (Vector3.Distance(GetPosition(), TargetIDamageable.GetPosition()) < _maxRange)
				return false;

			TargetIDamageable = null;
			return true;
        }

		private void OnTriggerEnter(Collider collider)
		{
			if(_isDebug)
				Debug.Log($"Minion {GetHashCode()} Found damageable [{collider.gameObject.GetComponent<IDamageable>() != null}]");

			IDamageable damageable;

			if((damageable = collider.gameObject.GetComponent<IDamageable>()) != null &&
			   TargetIDamageable != null && damageable.GetPriority() > TargetIDamageable.GetPriority())
				return;

			TargetIDamageable = damageable;
		}
	}
}
