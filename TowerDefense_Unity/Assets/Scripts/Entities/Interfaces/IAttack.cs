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
		/// Applies the effects of this attack upon the <see cref="IDamageable"/>
		/// given as parameter. This will: Damage, inflict movement impairing status.
		/// </summary>
		/// <param name="unitHit"> The unit that was hit.</param>
		void ApplyHitEffects(IDamageable unitHit);
	}
}