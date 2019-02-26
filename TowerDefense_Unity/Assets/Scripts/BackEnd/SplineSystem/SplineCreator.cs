using UnityEngine;

namespace Game.SplineSystem
{
    /// <summary>
    /// Holds control of all the splines.
    /// </summary>
    public class SplineCreator : MonoBehaviour
    {
        [SerializeField, HideInInspector] private BezierSplineDataObject _BezierSplineData;
        [SerializeField, HideInInspector] private int _SelectedPointIndex = 0;
        [SerializeField, HideInInspector] private bool _DrawTangents, _DrawNormals, _DrawBiNormals;
        [SerializeField, HideInInspector] private SplineMode _SplineSplineMode;

        public enum SplineMode
        {
            FREE,
            MIRRORED
        }

        public BezierSplineDataObject BezierSplineData
        {
            get => _BezierSplineData;
            set => _BezierSplineData = value;
        }
        public int SelectedPointIndex
        {
            get => _SelectedPointIndex;
            set => _SelectedPointIndex = value;
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

        public void ResetSpline()
        {
            _SplineSplineMode = SplineMode.MIRRORED;
            if (BezierSplineData == null) return;
            BezierSplineData.Reset(Vector3.zero);
        }
    }
}