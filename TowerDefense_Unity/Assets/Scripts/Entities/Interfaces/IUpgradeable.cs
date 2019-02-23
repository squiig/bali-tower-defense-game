namespace Game.Entities.Interfaces
{
	/// <summary>
	/// Interface representing that the inheritor can be upgraded by param T.
	/// </summary>
	/// <typeparam name="T"> The upgrade that can be applied to the inheritor.</typeparam>
	public interface IUpgradeable<T> where T : IUpgrade
	{
		/// <summary>
		/// Upgrades the instance by T.
		/// T must be gotten via <see cref="GetPossibleUpgrades"/>
		/// Upgrade can be rejected if not enough resources are available.
		/// </summary>
		/// <param name="upgrade"> Upgrade to apply to this instance.</param>
		void Upgrade(T upgrade);

		/// <summary>
		/// Returns an array of T containing all
		/// upgrades of T.
		/// Will never be null.
		/// </summary>
		/// <returns> An array of T with all possible upgrades. Never null.</returns>
		T[] GetPossibleUpgrades();
	}
}