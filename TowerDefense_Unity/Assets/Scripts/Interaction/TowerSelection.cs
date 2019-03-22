using UnityEngine;

namespace Game.Interaction
{
	/// <summary>
	/// Keeps track of which tower is selected inside of the tower gui. 
	/// </summary>
	public class TowerBuildSelection : DDOLMonoBehaviourSingleton<TowerBuildSelection>
	{
		private GameObject _SelectedObject;
		public GameObject SelectedObject { get; set; }
		public bool HasSelection => _SelectedObject != null;

		public event System.Action<GameObject> OnSelectionRecieved;
		public event System.Action<GameObject> OnSelectionCleared;

		public void ClearSelection()
		{
			OnSelectionCleared?.Invoke(_SelectedObject);
			_SelectedObject = null;
		}

		public GameObject TakeSelectionAndClear()
		{
			GameObject selected = _SelectedObject;
			ClearSelection();
			return selected;
		}

		public void SetTowerSelected(GameObject tower)
		{
			_SelectedObject = tower;
			OnSelectionRecieved?.Invoke(tower);
		}
	}
}

