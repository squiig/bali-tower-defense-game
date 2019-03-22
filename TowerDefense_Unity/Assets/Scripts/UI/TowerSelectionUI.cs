using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelectionUI : MonoBehaviour
{
	[SerializeField] private TowerSelectionDataObject[] _TowerSelectionDataObject;

	private void Awake()
	{
		for (int i = 0; i < _TowerSelectionDataObject.Length; i++)
		{
			GameObject towerSelection = Instantiate(_TowerSelectionDataObject[i].TowerUIPrefab);
			TowerSelectionOptionUI towerSelectionOptionUIScript = towerSelection.AddComponent<TowerSelectionOptionUI>();

			UnityEngine.UI.Button towerSelectionButton = towerSelection.AddComponent<UnityEngine.UI.Button>();

			Vector3 newPositionTowerSelectionOption = new Vector3(i * 150, 0, 0);

			//Reset the object to the local position and scale, and set the new position
			towerSelection.transform.parent = this.transform;
			towerSelection.transform.localPosition = newPositionTowerSelectionOption;
			towerSelection.transform.localScale = Vector3.one;

			towerSelectionOptionUIScript.Initialize(_TowerSelectionDataObject[i].TowerPrefab, towerSelectionButton);
		}
	}
}
