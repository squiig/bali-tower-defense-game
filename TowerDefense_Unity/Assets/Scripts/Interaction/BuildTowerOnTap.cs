using UnityEngine;
using Game.Turrets;

namespace Game.Interaction
{
	public class BuildTowerOnTap : MonoBehaviour, ITappable
	{
		private TowerBuildSelection _TowerBuildSelection;
		private TowerGridCell _TowerGridCell;

		public void Start()
		{
			_TowerBuildSelection = TowerBuildSelection.Instance;
			_TowerGridCell = gameObject.GetComponent<TowerGridCell>();
		}

		public void Tapped()
		{
			BuildTower();
		}

		private void BuildTower()
		{
			if (!_TowerBuildSelection.HasSelection || _TowerGridCell.IsOccupied)
				return;
			
			GameObject tower = Instantiate(_TowerBuildSelection.TakeSelectionAndClear());
			tower.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
			tower.transform.position = transform.position;
			_TowerGridCell.OccupyTurret(tower);
		}
	}
}
