using System;
using System.Linq;

namespace Game.Entities.MovingEntities
{
	public class AttackEffects : OnHitEffects
	{
		private readonly StatusEffects[] _statusEffects;
		private readonly float _damage;

		public AttackEffects(float damage, StatusEffects[] status)
		{
			_damage = damage;
			_statusEffects = status;
		}

		/// <inheritdoc />
		/// <summary>
		/// Array of status effects currently affecting this
		/// class. Will never return null.
		/// </summary>
		/// <returns> An array with all status effects.</returns>
		public override StatusEffects[] GetStatusEffects() => _statusEffects;

		/// <inheritdoc />
		/// <summary>
		/// Used to get the float value of damage inflicted.
		/// Will never return negative values.
		/// Will always return the absolute value.
		/// </summary>
		/// <returns> A positive number representing damage inflicted.</returns>
		public override float GetDamage() => _damage;

		/// <inheritdoc />
		/// <summary>
		/// Used to apply effects of the status effects.
		/// </summary>
		public override void ApplyStatusEffects()
		{
			if (_statusEffects.Any())
				throw new NotImplementedException("Have not implemented status effects yet.");
		}
	}
}