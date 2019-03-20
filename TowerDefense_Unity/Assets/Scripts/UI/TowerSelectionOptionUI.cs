using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelectionOptionUI : MonoBehaviour
{
	[SerializeField] private GameObject _PrefabToSpawnOnSelection;
	private UnityEngine.UI.Button _TowerSelectionOptionUIButton;

	private void OnEnable()
	{
		_TowerSelectionOptionUIButton = GetComponent<UnityEngine.UI.Button>();
		_TowerSelectionOptionUIButton.onClick.AddListener(OnTowerUIPress);
	}
	private void OnDisable()
	{
		_TowerSelectionOptionUIButton.onClick.RemoveListener(OnTowerUIPress);
	}

	public void SetPrefabAndImage(GameObject prefabToSpawn, UnityEngine.UI.Image towerSelectionOptionImage)
	{
		_PrefabToSpawnOnSelection = prefabToSpawn;
		_TowerSelectionOptionUIButton.image = towerSelectionOptionImage;
	}
	public void OnTowerUIPress()
	{
		Debug.Log("TowerUI Pressed");
		//Game.Interaction.TowerSelection.SelectTower(_PrefabToSpawnOnSelection);
	}
}
