namespace Game.Entities.Interfaces
{
	/// <summary>
	/// Interface representing that the inheritor can be upgraded by param T.
	/// </summary>
	/// <typeparam name="TUpgrade"> The upgrade that can be applied to the inheritor.</typeparam>
	/// <typeparam name="TUpgradeDependency">Class that has to be upgraded.</typeparam>
	public interface IUpgradeable<TUpgrade, TUpgradeDependency> 
		where TUpgrade : IUpgrade<TUpgradeDependency> where TUpgradeDependency : class
	{
		/// <summary>
		/// Upgrades the instance by T.
		/// T must be gotten via <see cref="GetPossibleUpgrades"/>
		/// Upgrade can be rejected if not enough resources are available.
		/// </summary>
		/// <param name="upgrade"> Upgrade to apply to this instance.</param>
		void Upgrade(TUpgrade upgrade);

		/// <summary>
		/// Returns an array of T containing all
		/// upgrades of T. If an upgrade is applied it will
		/// be removed from this list.
		/// Will never be null.
		/// </summary>
		/// <returns> An array of T with all possible upgrades. Never null.</returns>
		void GetPossibleUpgrades(out TUpgrade[] upgrades);
	}
}