using UnityEngine;

namespace Game.Interaction
{
	/// <summary>
	/// Keeps track of which tower is selected inside of the tower gui. 
	/// </summary>
	public class TowerBuildSelection : DDOLMonoBehaviourSingleton<TowerBuildSelection>
	{
		private GameObject _SelectedObject;
		public GameObject selectedObject { get; set; }
		public bool HasSelection { get => _SelectedObject != null; }

		public event System.Action<GameObject> OnSelectionRecieved;
		public event System.Action<GameObject> OnSelectionCleared;

		/// <summary>
		/// Clears the selection.
		/// </summary>
		public void Clear()
		{
			OnSelectionCleared?.Invoke(_SelectedObject);
			_SelectedObject = null;
		}

		/// <summary>
		/// Returns the currently selected tower and clears the selection.
		/// </summary>
		public GameObject Take()
		{
			GameObject selected = _SelectedObject;
			Clear();
			return selected;
		}

		/// <summary>
		/// Sets the current selected tower tto build
		/// </summary>
		public void SetTowerSelected(GameObject tower)
		{
			_SelectedObject = tower;
			OnSelectionRecieved?.Invoke(tower);
		}
	}
}

