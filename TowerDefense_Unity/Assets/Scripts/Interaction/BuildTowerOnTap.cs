using UnityEngine;
using Game.Turrets;

namespace Game.Interaction
{
	public class BuildTowerOnTap : MonoBehaviour/*, ITappable */
	{
		private TowerBuildSelection _TowerBuildSelection;
		private TurretGridCell _TurredGridCell;

		public void Start()
		{
			_TowerBuildSelection = TowerBuildSelection.Instance;
			_TurredGridCell = gameObject.GetComponent<TurretGridCell>();
		}

		public void Tapped()
		{
			BuildTower();
		}

		private void BuildTower()
		{
			if (!_TowerBuildSelection.HasSelection)
				return;
			if (_TurredGridCell.IsOccupied)
				return;
			
			GameObject tower = Instantiate(_TowerBuildSelection.Take());
			tower.transform.position = transform.position;
			_TurredGridCell.OccupyTurret(tower);
		}
	}
}
