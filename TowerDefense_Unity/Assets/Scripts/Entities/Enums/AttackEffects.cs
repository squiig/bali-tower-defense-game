using System;
using System.Linq;
using UnityEngine;

namespace Game.Entities.MovingEntities
{
	[CreateAssetMenu(fileName = "Attack Effects", menuName = "AttackEffects/Attack Effects", order = 1)]
	public class AttackEffects : OnHitEffects
	{
		[SerializeField] private StatusEffects[] _statusEffects;
		[SerializeField] private float _damage;

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
