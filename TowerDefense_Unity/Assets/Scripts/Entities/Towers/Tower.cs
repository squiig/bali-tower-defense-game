using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Entities;
using Game.Entities.Interfaces;
using Game.Entities.Towers;
using UnityEngine;

namespace Game.Entities.Towers
{
	public abstract class Tower : Entity, IAggressor, IUpgradeable<TowerRangeUpgrade, Tower>
	{
		private readonly float _startAttackRange;
		private readonly float _maxAttackRange;

		protected readonly IAttack Attack;

		protected float AttackRange;
		protected IDamageable TargetDamageable;

		protected Tower(float startAttackRange, int maxAttackRange, in IAttack attack)
		{
			_startAttackRange = startAttackRange;
			_maxAttackRange = maxAttackRange;

			Attack = attack;
		}

		/// <inheritdoc />
		/// <summary>
		/// Forcefully override the target of this entity.
		/// Target is set by the aggressor itself, but can
		/// be forcefully overriden at will.
		/// If target is out of range, this will reset to attack targets in range.
		/// </summary>
		/// <param name="target"> Target to focus upon.</param>
		public void SetTarget(in IDamageable target) => TargetDamageable = target;

		/// <inheritdoc />
		/// <summary>
		/// Used to forcefully attack the current target.
		/// If this instance has no target, target is out of
		/// range, or is reloading. This instance will stand idle.
		/// </summary>
		public abstract void ExecuteAttack();

		/// <summary>
        /// Increases the range of this turret.
        /// Should only ever be called by an upgrade.
        /// </summary>
        /// <param name="increase"></param>
		public void IncreaseRange(float increase)
		{
			AttackRange += increase;
			AttackRange = Mathf.Clamp(AttackRange, _startAttackRange, _maxAttackRange);
		}

		/// <inheritdoc />
		/// <summary>
		/// Used to get the attack of this IAggressor.
		/// Returns the attack class of this instance.
		/// </summary>
		/// <returns>Returns the attack class of this instance.</returns>
		public IAttack GetAttack() => Attack;

        /// <inheritdoc />
        /// <summary>
        /// Upgrades the instance by T.
        /// T must be gotten via <see cref="M:Game.Entities.Towers.Tower.GetPossibleUpgrades(TowerRangeUpgrade[]@)" />
        /// Upgrade can be rejected if not enough resources are available.
        /// </summary>
        /// <param name="upgrade"> Upgrade to apply to this instance.</param>
        public void Upgrade(in TowerRangeUpgrade upgrade)
		{
			ResourceSystem.Instance.RunTransaction(upgrade.GetCost());
			upgrade.ApplyUpgrade(this);
			throw new NotImplementedException("Transactions will always be false positive.");
		}

		/// <inheritdoc />
		/// <summary>
		/// Returns an array of T containing all
		/// upgrades of T. If an upgrade is applied it will
		/// be removed from this list.
		/// Will never be null.
		/// </summary>
		/// <returns> An array of T with all possible upgrades. Never null.</returns>
		public void GetPossibleUpgrades(out TowerRangeUpgrade[] upgrades)
		{
			upgrades = new TowerRangeUpgrade[0];
			throw new System.NotImplementedException("Upgrades have not been implemented yet.");
		}
	}
}

public class TowerRangeUpgrade : IUpgrade<Tower>
{
	private readonly float _upgradeValue = 20.0f;

	/// <inheritdoc />
	/// <summary>
	/// Applies an upgrade to the instance 
	/// </summary>
	public void ApplyUpgrade(in Tower instance) => instance.IncreaseRange(_upgradeValue);

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

