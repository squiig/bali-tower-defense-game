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
        [SerializeField] private Vector2 _CellSize, _CellResolution;
        private TurretGridCell[] _TurretGrid;

        private void Awake()
        {
            
        }
    }
}
