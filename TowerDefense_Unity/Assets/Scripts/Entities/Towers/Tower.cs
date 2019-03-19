using System;
using System.Collections.Generic;
using Game.Entities.Interfaces;
using UnityEngine;

namespace Game.Entities.Towers
{
	public class Tower : TowerBase, IUpgradeable
	{
		[SerializeField] private TowerUpgrade[] _Upgrades;

		/// <summary>
		/// Increases the range of this turret.
		/// Should only ever be called by an upgrade.
		/// </summary>
		/// <param name="increase"></param>
		public void IncreaseRange(float increase)
		{
			AttackRange += increase;
			AttackRange = Mathf.Clamp(AttackRange, StartAttackRange, MaxAttackRange);
		}

		public void IncreaseDamage(float increase)
		{
			throw new NotImplementedException("new system does not provide upgrades just yet");
			//Attack.
		}

		/// <inheritdoc />
		/// <summary>
		/// Upgrades the instance by T.
		/// T must be gotten via <see cref="GetPossibleUpgrades"/>
		/// Upgrade can be rejected if not enough resources are available.
		/// </summary>
		/// <param name="upgrade"> Upgrade to apply to this instance.</param>
		public void Upgrade(in IUpgrade upgrade)
		{
			if (!ResourceSystem.Instance.RunTransaction(upgrade.GetCost()))
				return;

			upgrade.ApplyUpgrade(this);
		}

		/// <inheritdoc />
		/// <summary>
		/// Returns an array of T containing all
		/// upgrades of T. If an upgrade is applied it will
		/// be removed from this list.
		/// Will never be null.
		/// </summary>
		/// <returns> An array of T with all possible upgrades. Never null.</returns>
		public void GetPossibleUpgrades(out TowerUpgrade[] upgrades)
		{
			if (_Upgrades == null)
			{
				upgrades = new TowerUpgrade[0];
				return;
			}

			upgrades = _Upgrades;
		}
	}
}
