using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Turrets
{
    /// <summary>Contains data of the gridcell itself.</summary>
    [SelectionBase]
    public class TurretGridCell : MonoBehaviour
    {
        private BoxCollider _BoxCollider;

        public bool IsOccupied { get; private set; }
        public GameObject Turret { get; private set; }

        private void Awake()
        {
            _BoxCollider = GetComponent<BoxCollider>();
        }

        public void SetColliderOffset(Vector2 position)
        {
            _BoxCollider.center = new Vector3(position.x, 0, position.y);
        }

        public void SetColliderSize(Vector2 size)
        {
            _BoxCollider.size = new Vector3(size.x, 0.1f, size.y);
        }

        public void OccupyTurret(GameObject turret)
        {
            if (turret == null)
            {
                Debug.LogError("The given turret doesn't exist!");
                return;
            }

            IsOccupied = true;
            Turret = turret;
        }

        public void EmptyGridCell()
        {
            IsOccupied = false;
            Turret = null;
        }
    }
}