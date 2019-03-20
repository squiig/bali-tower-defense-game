using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "new TowerSelectionObject", menuName = "", order = 1)]
public class TowerSelectionDataObject : ScriptableObject
{
	[SerializeField] [Tooltip("Select the image on the same index as the prefab")] private UnityEngine.UI.Image[] _TowerSelectionOptionImage;
	[SerializeField] [Tooltip("Select the image on the same index as the image")] private GameObject[] _TowerPrefab;

	private GameObject _DefaultTowerSelectionOptionPrefab;

	public UnityEngine.UI.Image[] TowerSelectionOptionSprite => _TowerSelectionOptionImage;
	public GameObject[] TowerPrefab => _TowerPrefab;

	private void OnEnable()
	{
		_DefaultTowerSelectionOptionPrefab = Resources.Load("Prefabs/UI/TowerSelectionOptions") as GameObject;
	}
}
