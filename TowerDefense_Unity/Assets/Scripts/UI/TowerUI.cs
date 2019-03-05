using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Game.UI
{
    public class TowerUI : MonoBehaviour
    {
		public event Action OnUpgrade;
		public event Action OnDestroy;

        [SerializeField] private TMP_Text _TitleText = null;
        [SerializeField] private ResourceUI _DamageHUD = null;
        [SerializeField] private ResourceUI _UpgradeCostHUD = null;
        [SerializeField] private Button _UpgradeButton = null;
        [SerializeField] private Button _DestroyButton = null;

		public void Refresh(TowerUIData uiData)
		{
			_TitleText.text = uiData.Title;
			_DamageHUD.SetAmount(uiData.Damage);
			_UpgradeCostHUD.SetAmount(uiData.UpgradeCost);
		}

		public void OnUpgradeButtonPressed()
		{
			OnUpgrade?.Invoke();
		}

		public void OnDestroyButtonPressed()
		{
			OnDestroy?.Invoke();
		}

		public struct TowerUIData
		{
			public string Title { get; set; }
			public int Damage { get; set; }
			public int UpgradeCost { get; set; }
		}
    }
}
