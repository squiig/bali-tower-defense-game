using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Turrets
{
    /// <summary>Holds the data of the turretgrid.</summary>
    public class TowerGrid : MonoBehaviour
    {
        [SerializeField] private Vector2 _CellSize = new Vector2Int(10, 10);
        [SerializeField] private Vector2Int _GridResolution = new Vector2Int(10, 10);
        [SerializeField] private TowerGridCell _TowerGridCellPrefab = null;

        private TowerGridCell[] _TowerGrid;

        public void Awake()
        {
            Vector2 gridSize = _CellSize * _GridResolution;
            _TowerGrid = new TowerGridCell[_GridResolution.x * _GridResolution.y];
            for (int i = 0, x = 0; x < _GridResolution.x; x++)
            {
                for (int z = 0; z < _GridResolution.y; z++, i++)
                {
                    GridPoint gridCellPoint = new GridPoint(new Vector3(-gridSize.x / 2 + x * _CellSize.x, 0, -gridSize.y / 2 + z * _CellSize.y), transform.eulerAngles);
                    _TowerGrid[i] = Instantiate(_TowerGridCellPrefab, transform.position + gridCellPoint.GetPointPosition(), transform.rotation, transform);
                    _TowerGrid[i].SetColliderSize(new Vector2(_CellSize.x, _CellSize.y));
                    _TowerGrid[i].SetColliderOffset(new Vector2(_CellSize.x / 2, _CellSize.y / 2));
                }
            }
        }

#if UNITY_EDITOR
		public void DrawHorizontalGridLines()
        {
            Vector3 position = transform.position;
            Vector3 rotation = transform.eulerAngles;
            Vector2 gridSize = _CellSize * _GridResolution;

            for (int z = 0; z < _GridResolution.y + 1; z++)
            {
                GridPoint startPoint = new GridPoint(new Vector3(-gridSize.x / 2, 0, z * _CellSize.y - gridSize.y / 2), rotation);
                GridPoint endPoint = new GridPoint(new Vector3(_GridResolution.x * _CellSize.x - gridSize.x / 2, 0, z * _CellSize.y - gridSize.y / 2), rotation);
                Handles.DrawLine(position + startPoint.GetPointPosition(), position + endPoint.GetPointPosition());
            }
        }

        public void DrawVerticalGridLines()
        {
            Vector3 position = transform.position;
            Vector3 rotation = transform.eulerAngles;
            Vector2 gridSize = _CellSize * _GridResolution;

            for (int x = 0; x < _GridResolution.x + 1; x++)
            {
                GridPoint startPoint = new GridPoint(new Vector3(x * _CellSize.x - gridSize.x / 2, 0, -gridSize.y / 2), rotation);
                GridPoint endPoint = new GridPoint(new Vector3(x * _CellSize.x - gridSize.x / 2, 0, _GridResolution.y * _CellSize.y - gridSize.y / 2), rotation);
                Handles.DrawLine(position + startPoint.GetPointPosition(), position + endPoint.GetPointPosition());
            }
        }
#endif
	}
}
