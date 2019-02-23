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
		void ApplyUpgrade<T>(IUpgradeable<T> instance) where T : IUpgrade;
	}
}