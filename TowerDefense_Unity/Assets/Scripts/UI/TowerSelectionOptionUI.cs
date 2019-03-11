using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelectionOptionUI : MonoBehaviour
{
	[SerializeField]
	private GameObject _PrefabToSpawnOnSelection;
	private UnityEngine.UI.Button _TowerSelectionOptionUIBUtton;

	private void Start()
	{
		_TowerSelectionOptionUIBUtton.onClick.AddListener(OnTowerUIPress);
	}

	public GameObject GetPrefabToSpawnOnSelection()
	{
		return _PrefabToSpawnOnSelection;
	}
	public void SetPrefabToSpawnOnSelection(GameObject prefabToSpawn)
	{
		_PrefabToSpawnOnSelection = prefabToSpawn;
	}

	public void OnTowerUIPress()
	{

	}
}
