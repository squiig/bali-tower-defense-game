using System;
using System.Collections.Generic;
using Game.Entities.EventContainers;
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
			
			if(Attack.GetAttackType() == AttackType.AREA_OF_EFFECT)
				Attack.SetAreaOfEffect(AttackRange + increase);
		}

		public void IncreaseDamage(float increase) => Attack.SetDamage(Attack.GetDamage() + increase);
		
		/// <inheritdoc />
		/// <summary>
		/// Upgrades the instance by T.
		/// T must be gotten via <see cref="GetPossibleUpgrades"/>
		/// Upgrade can be rejected if not enough resources are available.
		/// </summary>
		/// <param name="upgrade"> Upgrade to apply to this instance.</param>
		public void Upgrade(in IUpgrade upgrade, Action<TransactionResult> transactionCallback)
		{
			TransactionResult transaction = ResourceSystem.Instance.RunTransaction(-upgrade.GetCost());

			if (!transaction.HasTransactionFailed)
				upgrade.ApplyUpgrade(this);

			transactionCallback?.Invoke(transaction);
		}

		/// <inheritdoc />
		/// <summary>
		/// Returns an array of T containing all
		/// upgrades of T. If an upgrade is applied it will
		/// be removed from this list.
		/// Will never be null.
		/// </summary>
		/// <returns> An array of T with all possible upgrades. Never null.</returns>
		public TowerUpgrade[] GetPossibleUpgrades() => _Upgrades ?? new TowerUpgrade[0];
	}
}
