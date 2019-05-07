using Game.Entities.Towers;

namespace Game.Entities.Interfaces
{
	/// <summary>
	/// Interface representing an upgrade.
	/// </summary>
	public interface IUpgrade
	{
		/// <summary>
		/// Applies an upgrade to the instance 
		/// </summary>
		void ApplyUpgrade(in Tower instance);

		/// <summary>
        /// Used to request the cost of this upgrade.
        /// </summary>
        /// <returns> An integer representing the cost of this upgrade.</returns>
		int GetCost();

		/// <summary>
        /// Used to get a textual representation of this upgrade.
        /// </summary>
        /// <returns> A human readable representation of this upgrade.</returns>
		string GetDescription();
	}
}
