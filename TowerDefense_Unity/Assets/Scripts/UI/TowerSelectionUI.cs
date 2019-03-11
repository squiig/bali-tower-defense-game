using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelectionUI : MonoBehaviour
{
	[SerializeField] private GameObject[] _TowerPrefabs;
	[SerializeField] private GameObject _TowerSelectionOptionUIPrefab;

	private void Awake()
	{
		for (int i = 0; i < _TowerPrefabs.Length; i++)
		{
			GameObject towerSelection = Instantiate(_TowerSelectionOptionUIPrefab);
			TowerSelectionOptionUI towerSelectionOptionUIScript = towerSelection.AddComponent<TowerSelectionOptionUI>();

			Vector3 newPositionTowerSelectionOption = new Vector3(i * 150, 0, 0);

			towerSelection.transform.parent = this.transform;
			towerSelection.transform.localPosition = newPositionTowerSelectionOption;
			towerSelection.transform.localScale = Vector3.one;

			towerSelectionOptionUIScript.SetPrefabToSpawnOnSelection(_TowerPrefabs[i]);
		}
	}
}
