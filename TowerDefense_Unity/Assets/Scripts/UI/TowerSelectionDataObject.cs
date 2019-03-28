using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "new TowerSelectionObject", menuName = "", order = 1)]
public class TowerSelectionDataObject : ScriptableObject
{
	[SerializeField] [Tooltip("Select the tower UI prefab")] private GameObject _TowerUIPrefab;
	[SerializeField] [Tooltip("Select the tower prefab")] private GameObject _TowerPrefab;

	public GameObject TowerUIPrefab => _TowerUIPrefab;
	public GameObject TowerPrefab => _TowerPrefab;
}
