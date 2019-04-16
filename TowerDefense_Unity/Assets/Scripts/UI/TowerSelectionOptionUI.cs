using UnityEngine;
using UnityEngine.UI;
using Game.Interaction;

public class TowerSelectionOptionUI : MonoBehaviour
{
	[SerializeField] private Game.Entities.Towers.Tower _PrefabToSpawnOnSelection;
	[SerializeField] private TMPro.TextMeshProUGUI _TowerAttackText;
	[SerializeField] private TMPro.TextMeshProUGUI _TowerRangeText;
	[SerializeField] private TMPro.TextMeshProUGUI _TowerPriceText;

	[SerializeField] private int _TowerAttackAmount;
	[SerializeField] private int _TowerRangeAmount;
	[SerializeField] private int _TowerPrice;

	private Button _TowerSelectionOptionUIButton;
	private Game.ResourceSystem _ResourceSystem;

	private void OnEnable()
	{
		_TowerSelectionOptionUIButton = transform.GetComponentInChildren<Button>();
		_TowerSelectionOptionUIButton.onClick.AddListener(OnTowerUIPress);

		_TowerAttackAmount = (int)_PrefabToSpawnOnSelection.GetAttack().GetDamage();
		_TowerRangeAmount = (int)_PrefabToSpawnOnSelection.GetRange();
		_TowerPrice = (int)_PrefabToSpawnOnSelection.GetPrice();

		_TowerAttackText.text = _TowerAttackAmount.ToString();
		_TowerRangeText.text = _TowerRangeAmount.ToString();
		_TowerPriceText.text = _TowerPrice.ToString();

		_ResourceSystem = Game.ResourceSystem.Instance;
		_ResourceSystem.OnTransaction += Instance_OnTransaction;
	}

	private void Instance_OnTransaction(in Game.ResourceSystem sender, in Game.Entities.EventContainers.TransactionResult payload)
	{
		if (!payload.HasTransactionFailed)
			_TowerSelectionOptionUIButton.interactable = payload.ResourceCount >= _TowerPrice;
	}

	private void OnDisable()
	{
		_TowerSelectionOptionUIButton.onClick.RemoveListener(OnTowerUIPress);
	}

	public void OnTowerUIPress()
	{
		if (_ResourceSystem.ResourceCount >= _TowerPrice)
		{
			TowerBuildSelection.Instance.SetTowerSelected(_PrefabToSpawnOnSelection.gameObject);
		}
	}
}
