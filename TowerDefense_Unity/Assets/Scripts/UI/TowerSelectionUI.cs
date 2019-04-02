using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelectionUI : MonoBehaviour
{
	[SerializeField] private TowerSelectionDataObject[] _TowerSelectionDataObject;
	[SerializeField] private float _MarginXBetweenTowers;

	private void Awake()
	{
		RectTransform previousButton = null;

		for (int i = 0; i < _TowerSelectionDataObject.Length; i++)
		{
			GameObject towerSelection = Instantiate(_TowerSelectionDataObject[i].TowerUIPrefab);
			TowerSelectionOptionUI towerSelectionOptionUIScript = towerSelection.AddComponent<TowerSelectionOptionUI>();
			Vector3 newPositionTowerSelectionOption = Vector3.zero;

			UnityEngine.UI.Button towerSelectionButton = towerSelection.AddComponent<UnityEngine.UI.Button>();

			if (previousButton != null)
			{
				newPositionTowerSelectionOption.x = (i * previousButton.sizeDelta.x) + _MarginXBetweenTowers;
			}

			//Reset the object to the local position and scale, and set the new position
			towerSelection.transform.SetParent(this.transform);
			towerSelection.transform.localPosition = newPositionTowerSelectionOption;

			towerSelectionOptionUIScript.Initialize(_TowerSelectionDataObject[i].TowerPrefab, towerSelectionButton);

			previousButton = towerSelection.GetComponent<RectTransform>();
		}
	}
}
