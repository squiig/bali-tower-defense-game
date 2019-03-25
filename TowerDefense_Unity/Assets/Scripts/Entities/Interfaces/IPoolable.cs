namespace Game.Entities.Interfaces
{
	public interface IPoolable
	{
		/// <summary>
		/// Used to check whether this object is currently in the game
		/// or is only available in the pool.
		/// [TRUE] if in scene, [FALSE] if only available from pool.
		/// </summary>
		/// <returns> [TRUE] if in scene, [FALSE] if only available from pool.</returns>
		bool IsConducting();

		/// <summary>
		/// Activate this instance. Called when an object has been requested from
		/// the pool.
		/// </summary>
		void Activate();

		/// <summary>
		/// Releases ownership of this object and returns it to the pool.
		/// </summary>
		void ReleaseOwnership();
	}
}