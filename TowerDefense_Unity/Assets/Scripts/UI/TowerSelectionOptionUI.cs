using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelectionOptionUI : MonoBehaviour
{
	[SerializeField] private GameObject _PrefabToSpawnOnSelection;
	private UnityEngine.UI.Button _TowerSelectionOptionUIButton;

	/// <summary>
	/// Initialize the Tower Selection Option
	/// </summary>
	/// <param name="prefabToSpawn">The</param>
	/// <param name="towerSelectionOptionUIButton">The button component from this object</param>
	public void Initialize(GameObject prefabToSpawn, UnityEngine.UI.Button towerSelectionOptionUIButton)
	{
		_PrefabToSpawnOnSelection = prefabToSpawn;
		_TowerSelectionOptionUIButton = towerSelectionOptionUIButton;

		_TowerSelectionOptionUIButton.onClick.AddListener(OnTowerUIPress);
	}

	private void OnDisable()
	{
		_TowerSelectionOptionUIButton.onClick.RemoveListener(OnTowerUIPress);
	}

	public void OnTowerUIPress()
	{
		Game.Interaction.TowerBuildSelection.Instance.SetTowerSelected(_PrefabToSpawnOnSelection);
	}
}
