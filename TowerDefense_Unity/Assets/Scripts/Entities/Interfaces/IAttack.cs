using System.ComponentModel;
using UnityEngine;

namespace Game.Entities.Interfaces
{
	/// <summary>
	/// Interface representing an attack.
	/// </summary>
	public interface IAttack
	{
		/// <summary>
		/// Used to gain the attack type of this instance.
		/// </summary>
		/// <returns> The attack type of this instance.</returns>
		AttackType GetAttackType();

		/// <summary>
		/// Gets the area of effect of this attack.
		/// If this instance does not do AoE attacks,
		/// this will always return -1.
		/// </summary>
		/// <returns> The float representing the attacks range. -1 if not applicable.</returns>
		float GetAreaOfEffect();

		/// <summary>
        /// Executes the attack on the damageable given.
        /// </summary>
		void ExecuteAttack(in IDamageable damageable, Vector3? positionOverride = null);

	}
}