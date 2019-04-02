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
			//Add the TowerSelectionOptionUI script to the image prefab, since it won't have it at the start
			TowerSelectionOptionUI towerSelectionOptionUIScript = towerSelection.AddComponent<TowerSelectionOptionUI>();
			Vector3 newPositionTowerSelectionOption = Vector3.zero;

			//Add the button component to the image prefab, since it won't have it at the start
			UnityEngine.UI.Button towerSelectionButton = towerSelection.AddComponent<UnityEngine.UI.Button>();

			//If we have spawned a button before, use his position to determine where the new button must be created
			if (previousButton != null)
				newPositionTowerSelectionOption.x = (i * previousButton.sizeDelta.x) + _MarginXBetweenTowers;

			//Reset the object to the local position and scale, and set the new position
			towerSelection.transform.SetParent(this.transform);
			towerSelection.transform.localPosition = newPositionTowerSelectionOption;

			//Start the tower selection option and give the prefab from the Data Object to him and the button component
			towerSelectionOptionUIScript.Initialize(_TowerSelectionDataObject[i].TowerPrefab, towerSelectionButton);

			previousButton = towerSelection.GetComponent<RectTransform>();
		}
	}
}
