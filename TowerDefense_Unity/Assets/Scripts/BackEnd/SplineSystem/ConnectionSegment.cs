using UnityEditor;
using UnityEngine;

namespace Game.SplineSystem
{
	/// <summary>
	/// The connection curve is generated when a spline wants to split or merge with another spline.
	/// It contains references to both spline objects and needs it's draw method to be called.
	/// </summary>
	[System.Serializable]
	public class ConnectionSegment
	{
		[SerializeField] private Vector3[] _Points = new Vector3[4];
		[SerializeField] private BezierSplineDataObject _SenderSpline, _ReceiverSpline;
		[SerializeField] private int _StartHandleIndex, _EndHandleIndex;

		public Vector3 this[int i] => _Points[i];
		public BezierSplineDataObject SenderSpline => _SenderSpline;
		public BezierSplineDataObject ReceiverSpline => _ReceiverSpline;

		public void SetCurve(int startHandleIndex, int endHandleIndex, BezierSplineDataObject sender, BezierSplineDataObject receiver)
		{
			_StartHandleIndex = startHandleIndex;
			_EndHandleIndex = endHandleIndex;
			_SenderSpline = sender;
			_ReceiverSpline = receiver;
		}

		public void Draw(Color color)
		{
			_Points[0] = _SenderSpline[_StartHandleIndex];
			_Points[1] = _SenderSpline[_SenderSpline.PointCount - 1] * 2 - _SenderSpline[_SenderSpline.PointCount - 2];
			_Points[2] = _EndHandleIndex > 0 ? _ReceiverSpline[_EndHandleIndex - 1] : _ReceiverSpline[_EndHandleIndex + 1] * -1;
			_Points[3] = _ReceiverSpline[_EndHandleIndex];

			Handles.DrawBezier(_Points[0], _Points[3], _Points[1], _Points[2], color, null, 4f);
		}
	}
}
