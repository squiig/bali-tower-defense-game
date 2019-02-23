using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.BackEnd.Turrets
{
    /// <summary>Contains data of the gridcell itself.</summary>
    public class TurretGridCell
    {
        public bool _IsOccupied { get; set; }
    }

    /// <summary>Holds the data of the turretgrid.</summary>
    public class TurretGrid : MonoBehaviour
    {
        [SerializeField] private Vector2Int _CellSize = new Vector2Int(10, 10);
        [SerializeField] private Vector2Int _GridResolution = new Vector2Int(10, 10);
        private TurretGridCell[] _TurretGrid;

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
    }
}
