using Game.Entities.Interfaces;
using Game.Entities.Towers;

namespace Game.Entities.Towers
{
	public class TowerUpgrade : IUpgrade
	{
		private readonly float _upgradeValue = 20.0f;

		/// <inheritdoc />
		/// <summary>
		/// Applies an upgrade to the instance 
		/// </summary>
		public void ApplyUpgrade(in Tower instance)
		{
			instance.IncreaseRange(_upgradeValue);
			instance.IncreaseDamage(_upgradeValue);
		}

		/// <inheritdoc />
		/// <summary>
		/// Used to request the cost of this upgrade.
		/// </summary>
		/// <returns> An integer representing the cost of this upgrade.</returns>
		public int GetCost() => 200;

		/// <inheritdoc />
		/// <summary>
		/// Used to get a textual representation of this upgrade.
		/// </summary>
		/// <returns> A human readable representation of this upgrade.</returns>
		public string GetDescription() => "Increases the damage of the turret";
	}
}
