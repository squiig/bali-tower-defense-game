using UnityEngine;
using UnityEngine.UI;
using Game.Interaction;

public class TowerSelectionOptionUI : MonoBehaviour
{
	[SerializeField] private GameObject _PrefabToSpawnOnSelection;
	[SerializeField]

	private Button _TowerSelectionOptionUIButton;

	private void OnEnable()
	{
		_TowerSelectionOptionUIButton = transform.GetComponentInChildren<Button>();
		_TowerSelectionOptionUIButton.onClick.AddListener(OnTowerUIPress);
	}

	private void OnDisable()
	{
		_TowerSelectionOptionUIButton.onClick.RemoveListener(OnTowerUIPress);
	}

	public void OnTowerUIPress()
	{
		TowerBuildSelection.Instance.SetTowerSelected(_PrefabToSpawnOnSelection);
	}
}
