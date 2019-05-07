namespace Game.Entities.Interfaces
{
	/// <summary>
	/// Interface representing all entities that attack.
	/// </summary>
	public interface IAggressor
	{
		/// <summary>
		/// Forcefully override the target of this entity.
		/// Target is set by the aggressor itself, but can
		/// be forcefully overriden at will.
		/// If target is out of range, this will reset to attack targets in range.
		/// </summary>
		/// <param name="target"> Target to focus upon.</param>
		void SetTarget(in IDamageable target);

		/// <summary>
		/// Used to forcefully attack the current target.
		/// If this instance has no target, target is out of
		/// range, or is reloading. This instance will stand idle.
		/// </summary>
		void ExecuteAttack();

		/// <summary>
		/// Used to get the attack of this IAggressor.
		/// Returns the attack class of this instance.
		/// </summary>
		/// <returns>Returns the attack class of this instance.</returns>
		IAttack GetAttack();
	}
}
