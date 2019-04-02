using System;
using Game.Entities.EventContainers;
using Game.Entities.Towers;

namespace Game.Entities.Interfaces
{
	/// <summary>
	/// Interface representing that the inheritor can be upgraded
	/// </summary>
	public interface IUpgradeable
	{
		/// <summary>
		/// Upgrades the instance by T.
		/// T must be gotten via <see cref="GetPossibleUpgrades"/>
		/// Upgrade can be rejected if not enough resources are available.
		/// </summary>
		/// <param name="upgrade"> Upgrade to apply to this instance.</param>
		void Upgrade(in IUpgrade upgrade, Action<TransactionResult> transactionCallback);

		/// <summary>
		/// Returns an array of T containing all
		/// upgrades of T. If an upgrade is applied it will
		/// be removed from this list.
		/// Will never be null.
		/// </summary>
		/// <returns> An array of T with all possible upgrades. Never null.</returns>
		TowerUpgrade[] GetPossibleUpgrades();
	}
}
