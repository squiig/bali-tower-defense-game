using System;
using System.Data;

namespace Game.Entities
{
	/// <summary>
    /// Class representing the contents of
    /// what to apply when hit.
    /// </summary>
	public abstract class OnHitEffects
	{
		/// <summary>
        /// Array of status effects currently affecting this
        /// class. Will never return null.
        /// </summary>
        /// <returns> An array with all status effects.</returns>
		public abstract StatusEffects[] GetStatusEffects();

		/// <summary>
        /// Used to get the float value of damage inflicted.
        /// Will never return negative values.
        /// Will always return the absolute value.
        /// </summary>
        /// <returns> A positive number representing damage inflicted.</returns>
		public abstract float GetDamage();	

		/// <summary>
        /// Used to apply effects of the status effects.
        /// </summary>
		public abstract void ApplyStatusEffects();
	}
}
