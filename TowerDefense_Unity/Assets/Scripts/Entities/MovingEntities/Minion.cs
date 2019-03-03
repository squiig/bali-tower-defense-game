using Game.Entities;
using Game.Entities.Interfaces;
using UnityEngine;

namespace Game.Entities.MovingEntities
{
	public class Minion : TraversingEntity
	{
		private readonly float _range;

		private float _attackCoolDown = 0;
		private float _attackCoolDownDuration = 3.0f;

		public Minion(float maxHealth, float range, int priority, in IAttack attack) :
			base(maxHealth, priority, attack) => _range = range;

		private void Update()
		{
			if (_attackCoolDown > 0)
				_attackCoolDown -= Time.unscaledDeltaTime;

			ExecuteAttack();
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

			if(Vector3.Distance(GetPosition(), TargetIDamageable.GetPosition()) > _range)
				return;

			_attackCoolDown = _attackCoolDownDuration;
			Attack.ExecuteAttack(TargetIDamageable);
		}
	}
}
