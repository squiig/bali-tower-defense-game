using UnityEngine;
using Game.Turrets;

namespace Game.Interaction
{
	public class BuildTowerOnTap : MonoBehaviour, ITappable
	{
		private TowerBuildSelection _TowerBuildSelection;
		private TurretGridCell _TurretGridCell;

		public void Start()
		{
			_TowerBuildSelection = TowerBuildSelection.Instance;
			_TurretGridCell = gameObject.GetComponent<TurretGridCell>();
		}

		public void Tapped()
		{
			BuildTower();
		}

		private void BuildTower()
		{
			if (!_TowerBuildSelection.HasSelection || _TurretGridCell.IsOccupied)
				return;
			
			GameObject tower = Instantiate(_TowerBuildSelection.TakeSelectionAndClear());
			tower.transform.position = transform.position;
			_TurretGridCell.OccupyTurret(tower);
		}
	}
}
