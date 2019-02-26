using UnityEngine;

namespace Game.SplineSystem
{
    /// <summary>
    /// Holds control of all the splines.
    /// </summary>
    public class SplineCreator : MonoBehaviour
    {
        [SerializeField] private BezierSplineDataObject _BezierSplineData;
        [SerializeField] private int _SelectedPointIndex = 0;

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

        public void ResetSpline()
        {
            if (BezierSplineData == null) return;
            BezierSplineData.Reset(Vector3.zero);
        }
    }
}