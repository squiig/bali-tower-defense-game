using System.Collections.Generic;
using UnityEngine;

namespace Game.SplineSystem
{
    /// <summary>Contains the data a selected point in one of the spline branches.</summary>
    public struct HandlePointIndex
    {
        public int HandleIndex { get; set; }
        public int BranchIndex { get; set; }
    }

    /// <summary>A spline branch manager which, keeps track of the spline branches, which are represented as child objects.</summary>
    public class SplineCreator : SplineCreatorBase
    {
        [SerializeField, HideInInspector] private SplineBranchCreator[] _Branches = new SplineBranchCreator[0];
        [SerializeField, HideInInspector] private Vector3 _Position;
        [SerializeField, HideInInspector] private HandlePointIndex _SelectedPointIndex, _ModifierPointIndex;
	    [SerializeField, HideInInspector] private List<ConnectionSegment> _ConnectionSegments = new List<ConnectionSegment>();
		private System.Action<ConnectionSegment> _ConnectionSegmentDestroyCallback;

	    public SplineBranchCreator this[int i] => _Branches[i];
		public ConnectionSegment GetConnectionSegment(int i) => _ConnectionSegments[i];
	    public int ConnectionSegmentCount => _ConnectionSegments.Count;
        public int BranchCount => Branches.Length;
        public SplineBranchCreator[] Branches
        {
            get => _Branches;
            set => _Branches = value;
        }
        public Vector3 Position
        {
            get => _Position;
            set => _Position = value;
        }
        public HandlePointIndex SelectedPointIndex
        {
            get => _SelectedPointIndex;
            set => _SelectedPointIndex = value;
        }
        public HandlePointIndex ModifierPointIndex
        {
            get => _ModifierPointIndex;
            set => _ModifierPointIndex = value;
        }

	    public void AddConnectionSegment(int selectedHandleIndex, int modifierHandleIndex, BezierSplineDataObject senderSpline, BezierSplineDataObject receiverSpline)
	    {
		    if (_ConnectionSegmentDestroyCallback == null) _ConnectionSegmentDestroyCallback = RemoveConnectionSegment;

		    ConnectionSegment connectionSegment = new ConnectionSegment(_ConnectionSegmentDestroyCallback);
		    connectionSegment.SetCurve(selectedHandleIndex, modifierHandleIndex, senderSpline, receiverSpline);
		    _ConnectionSegments.Add(connectionSegment);
	    }

	    public void RemoveConnectionSegment(ConnectionSegment connectionSegment)
	    {
		    if (_ConnectionSegments.Contains(connectionSegment))
			    _ConnectionSegments.Remove(connectionSegment);
	    }
	}
}
