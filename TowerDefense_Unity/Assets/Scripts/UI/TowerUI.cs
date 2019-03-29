using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Entities.Towers;
using Game.Entities.Interfaces;
using Game.Entities.EventContainers;

namespace Game.UI
{
	/// <summary>
	/// Manages all UI components of individual tower selecting.
	/// </summary>
    public class TowerUI : MonoBehaviour
    {
		[SerializeField] private Tower _Tower = null;
        [SerializeField] private TMP_Text _TitleRenderer = null;
		[SerializeField] private ResourceUI _DamageUI = null;
		[SerializeField] private ResourceUI _RangeUI = null;
		[SerializeField] private ResourceUI _UpgradeCostUI = null;
        [SerializeField] private Button _UpgradeButton = null;

		private int? GetDamage()
		{
			IAttack attack = _Tower.GetAttack();
			return attack == null ? null : (int?)Mathf.RoundToInt(attack.GetDamage());
		}

		private int? GetUpgradeCost()
		{
			TowerUpgrade[] upgrades = _Tower.GetPossibleUpgrades();
			return upgrades.Length <= 0 ? null : upgrades[0] == null ? null : (int?)upgrades[0].GetCost();
		}

		private void OnEnable()
		{
			_UpgradeButton.onClick.AddListener(OnUpgradeButtonPressed);
			ResourceSystem.Instance.OnTransaction += ResourceSystem_OnTransaction;
		}

		private void ResourceSystem_OnTransaction(in ResourceSystem sender, in TransactionResult payload)
		{
			if (payload.ResourceCount > GetUpgradeCost())
			{
				_UpgradeButton.interactable = true;
			}
		}

		private void Start()
		{
			Refresh();
		}

		private void OnDisable()
		{
			_UpgradeButton.onClick.RemoveListener(OnUpgradeButtonPressed);
		}

		public void OnUpgradeButtonPressed()
		{
			_Tower.Upgrade(_Tower.GetPossibleUpgrades()[0], (transactionResult) => 
			{
				if (transactionResult.HasTransactionFailed)
				{
					//TODO: message player about impossible upgrade
				}
				else
				{
					Refresh();
				}
			});
		}

		public void Refresh()
		{
			if (_Tower == null)
				return;

			// I was unsure how to do this elegantly, might take another look at it after first release
			_TitleRenderer.text = "Towery";
			_DamageUI.SetAmount(GetDamage() ?? 0);
			_RangeUI.SetAmount(Mathf.RoundToInt(_Tower.GetRange()));
			_UpgradeCostUI.SetAmount(GetUpgradeCost() ?? 0);

			if (ResourceSystem.Instance.ResourceCount < GetUpgradeCost())
			{
				_UpgradeButton.interactable = false;
			}
		}
    }
}
