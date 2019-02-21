using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.BackEnd.Turrets
{
    /// <summary>Contains data of the gridcell itself.</summary>
    public struct TurretGridCell
    {
        public bool _IsOccupied { get; set; }
    }

    /// <summary>Holds the data of the turretgrid.</summary>
    public class TurretGrid : MonoBehaviour
    {
        [Header("Gridsize options")]
        [SerializeField] private Vector2Int _CellSize = new Vector2Int(10, 10);
        [SerializeField] private Vector2Int _GridResolution = new Vector2Int(10, 10);
        private TurretGridCell[] _TurretGrid;

        private void Awake()
        {
            
        }
    }
}
