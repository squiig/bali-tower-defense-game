using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.SplineSystem
{
	/// <summary>
	/// Holds all the data regarding a bezierSpline.
	/// </summary>
	[CreateAssetMenu(fileName = "new BezierSpline", menuName = "Bezier Spline", order = 0)]
	public class BezierSplineDataObject : ScriptableObject
	{
		[SerializeField] private bool _Use = true;
		[SerializeField] private Vector3 _Position = Vector3.zero;
		[SerializeField] private bool _IsClosed;
		[SerializeField] private List<Vector3> _Points = new List<Vector3>();

		public bool Use => _Use;

		public Vector3 this[int i]
		{
			get => _Points[i];
			set => _Points[i] = value;
		}
		public Vector3[] GetSegmentPoints(int segmentIndex) => new[] { _Points[segmentIndex * 3], _Points[segmentIndex * 3 + 1], _Points[segmentIndex * 3 + 2], _Points[LoopIndex(segmentIndex * 3 + 3)] };
		public int SegmentCount => _Points.Count / 3;
		public int PointCount => _Points.Count;
		public bool IsClosed
		{
			get => _IsClosed;
			set => _IsClosed = value;
		}
		public Vector3 Position
		{
			get => _Position;
			set => _Position = value;
		}

        private int LoopIndex(int index) => (index + _Points.Count) % _Points.Count;

        public void Reset(Vector3 center)
        {
            _Position = Vector3.zero;
            _IsClosed = false;
            _Points = new List<Vector3>()
            {
                center + Vector3.left * 10.0f,
                center + (Vector3.left + Vector3.back) * 5.0f,
                center + (Vector3.right + Vector3.forward) * 5.0f,
                center + Vector3.right * 10.0f
            };
        }

        public void AddSegment(Vector3 segmentEndPos)
        {
            _Points.Add(_Points[_Points.Count - 1] * 2 - _Points[_Points.Count - 2]);
            _Points.Add((_Points[_Points.Count - 1] + segmentEndPos) * 0.5f);
            _Points.Add(segmentEndPos);
        }

        public void MovePoint(int index, Vector3 point, SplineCreatorBase.SplineMode mode)
        {
            if (index % 3 == 0)
            {
                Vector3 delta = point - _Points[index];
                _Points[index] = point;
                if (index - 1 >= 0 || _IsClosed) _Points[LoopIndex(index - 1)] += delta;
                if (index + 1 < _Points.Count || _IsClosed) _Points[LoopIndex(index + 1)] += delta;
                return;
            }

            switch (mode)
            {
                case SplineBranchCreator.SplineMode.FREE:
                    _Points[index] = point;
                    break;
                case SplineBranchCreator.SplineMode.MIRRORED:
                    _Points[index] = point;

                    bool isNextPointAnchor = (index + 1) % 3 == 0;
                    int correspondingControlIndex = isNextPointAnchor ? index + 2 : index - 2;
                    int anchorIndex = isNextPointAnchor ? index + 1 : index - 1;

                    if (correspondingControlIndex >= 0 && correspondingControlIndex < _Points.Count - 1 || _IsClosed)
                    {
                        float length = (_Points[LoopIndex(anchorIndex)] - _Points[LoopIndex(correspondingControlIndex)]).magnitude;
                        Vector3 direction = (_Points[LoopIndex(anchorIndex)] - point).normalized;
                        _Points[LoopIndex(correspondingControlIndex)] = _Points[LoopIndex(anchorIndex)] + direction * length;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void RemoveSegment(int controlPointIndex)
        {
            if (controlPointIndex % 3 != 0 || SegmentCount <= 1)
	            return;

            if (controlPointIndex == 0)
            {
                if (IsClosed)
                    _Points[_Points.Count - 1] = _Points[2];
                _Points.RemoveRange(controlPointIndex, 3);
            }
            else if (controlPointIndex == _Points.Count - 1)
                _Points.RemoveRange(controlPointIndex - 2, 3);
            else
                _Points.RemoveRange(controlPointIndex - 1, 3);
        }

        public void ToggleClosed(bool closed)
        {
            _IsClosed = closed;
	        if (closed)
	        {
		        _Points.Add(_Points[_Points.Count - 4] * 2 - _Points[_Points.Count - 5]);
		        _Points.Add(_Points[0] * 2 - _Points[1]);
		        return;
	        }
	        _Points.RemoveRange(_Points.Count - 2, 2);
	        return;
        }

        public void InsertSegment(int pointIndex, Vector3 point)
        {
            Vector3[] newAnchorPoint = { point + Vector3.left, point, point + Vector3.right };
            _Points.InsertRange(pointIndex + 2, newAnchorPoint);
        }

        public Vector3 GetCenterPoint()
        {
            Vector3 totalPos = Vector3.zero;
            foreach (Vector3 point in _Points)
                totalPos += point;
            return _Position = totalPos / PointCount;
        }
    }
}
