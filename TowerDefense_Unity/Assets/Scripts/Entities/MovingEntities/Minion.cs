using System.Linq;
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
		[SerializeField] private Allegiance _allegiance;
		[Space]
		[SerializeField] private bool _isDebug;

		private void Awake()
		{
			Initialize(_maxHealth, _attackPriority, _allegiance, new MinionAttack());
        }

		private void Update()
		{
			base.Update();
			
            if (_isDebug)
                DrawDebug();

            TargetingAndAttacks();
        }

        private void TargetingAndAttacks()
        {
            if (TargetIDamageable == null)
                GetNewTarget(out TargetIDamageable);

            if (_attackCoolDown > 0)
                _attackCoolDown -= Time.unscaledDeltaTime;

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

		/// <summary>
        /// Sets a new target. Does not check if target isn't already set.
        /// if it is will be overriden with a new target or null.
        /// </summary>
        /// <param name="target"></param>
        private void GetNewTarget(out IDamageable target)
		{
			//TODO: Not quite what we want, large performance impact.
			target = MemoryObjectPool<IDamageable>.Instance.
				Where(x=> x.IsConducting() && 
						  x.GetAllegiance() != GetAllegiance() &&
				          Vector3.Distance(GetPosition(), x.GetPosition()) < _maxRange).
				OrderByDescending(x => x.GetPriority()).
				ThenBy(x => Vector3.Distance(GetPosition(), x.GetPosition())).
				FirstOrDefault(x => !ReferenceEquals(x, this));
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
			Attack.ExecuteAttack(TargetIDamageable);

			ReleaseOwnership();
		}

		private bool IsTargetForsaken()
		{
			if (Vector3.Distance(GetPosition(), TargetIDamageable.GetPosition()) < _maxRange)
				return false;

			TargetIDamageable = null;
			return true;
        }
	}
}
