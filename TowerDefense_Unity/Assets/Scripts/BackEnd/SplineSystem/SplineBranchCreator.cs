using UnityEngine;

namespace Game.SplineSystem
{
    /// <summary>
    /// Holds control of all the splines.
    /// </summary>
    [SelectionBase]
    public class SplineBranchCreator : SplineCreatorBase
    {
        [SerializeField, HideInInspector] private BezierSplineDataObject _BezierSplineData;
        [SerializeField, HideInInspector] private int _SelectedPointIndex = 0;

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

        public override void Reset()
        {
            _SplineSplineMode = SplineMode.MIRRORED;
            transform.position = Vector3.zero;
            _SelectedPointIndex = 0;
            if (BezierSplineData == null)
	            return;
            BezierSplineData.Reset(Vector3.zero);
        }
    }
}
