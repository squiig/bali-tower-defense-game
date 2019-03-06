using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.SplineSystem
{
    /// <summary>Serves as a base class with basic variables for later purpose.</summary>
    public abstract class SplineCreatorBase : MonoBehaviour
    {
        [SerializeField, HideInInspector] protected bool _DrawTangents, _DrawNormals, _DrawBiNormals;
        [SerializeField, HideInInspector] protected SplineMode _SplineSplineMode;

        public enum SplineMode
        {
            FREE,
            MIRRORED
        }

        public bool DrawTangents
        {
            get => _DrawTangents;
            set => _DrawTangents = value;
        }
        public bool DrawNormals
        {
            get => _DrawNormals;
            set => _DrawNormals = value;
        }
        public bool DrawBiNormals
        {
            get => _DrawBiNormals;
            set => _DrawBiNormals = value;
        }
        public SplineMode DrawMode
        {
            get => _SplineSplineMode;
            set => _SplineSplineMode = value;
        }

        public virtual void Reset(){}
    }
}
