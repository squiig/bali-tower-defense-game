using UnityEngine;

namespace Game.BackEnd.SplineSystem
{
    /// <summary>
    /// Holds control of all the splines.
    /// </summary>
    public class SplineCreator : MonoBehaviour
    {
        [SerializeField] private BezierSplineDataObject _BezierSplineData;

        public BezierSplineDataObject BezierSplineData
        {
            get => _BezierSplineData;
            set => _BezierSplineData = value;
        }
        public int SelectedPoint { get; set; } = 0;

        public void ResetSpline()
        {
            if (BezierSplineData == null) return;
            BezierSplineData.Reset(transform.position);
        }
    }
}