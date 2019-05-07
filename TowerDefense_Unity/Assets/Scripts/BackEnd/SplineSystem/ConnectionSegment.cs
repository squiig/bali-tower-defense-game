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
		private readonly System.Action<ConnectionSegment> _DestroyCallback;

		public Vector3 this[int i]
		{
			get => _Points[i];
			set => _Points[i] = value;
		}
		public BezierSplineDataObject SenderSpline => _SenderSpline;
		public BezierSplineDataObject ReceiverSpline => _ReceiverSpline;
		public int SenderHandleIndex => _StartHandleIndex;
		public int ReceiverHandleIndex => _EndHandleIndex;

		public ConnectionSegment(System.Action<ConnectionSegment> destroyCallback)
		{
			_DestroyCallback = destroyCallback;
		}

		public void SetCurve(int startHandleIndex, int endHandleIndex, BezierSplineDataObject sender, BezierSplineDataObject receiver)
		{
			_StartHandleIndex = startHandleIndex;
			_EndHandleIndex = endHandleIndex;
			_SenderSpline = sender;
			_ReceiverSpline = receiver;
		}

		public void Update()
		{
			if (_ReceiverSpline == null || _SenderSpline == null)
			{
				_DestroyCallback?.Invoke(this);
				return;
			}

			_Points[0] = _SenderSpline[_StartHandleIndex];
			_Points[1] = GetFirstControlPoint();
			_Points[2] = GetSecondControlPoint();
			_Points[3] = _ReceiverSpline[_EndHandleIndex];
		}

		public Vector3 GetFirstControlPoint()
		{
			float delta;
			Vector3 direction;

			if (_StartHandleIndex >= _SenderSpline.PointCount - 1)
			{
				delta = (_SenderSpline[_StartHandleIndex] - _SenderSpline[_StartHandleIndex - 1]).magnitude;
				direction = (_SenderSpline[_StartHandleIndex] - _SenderSpline[_StartHandleIndex - 1]).normalized;
				return _SenderSpline[_StartHandleIndex] + direction * delta;
			}

			delta = (_SenderSpline[_StartHandleIndex] - _SenderSpline[_StartHandleIndex + 1]).magnitude;
			direction = (_SenderSpline[_StartHandleIndex] - _SenderSpline[_StartHandleIndex + 1]).normalized;
			return _SenderSpline[_StartHandleIndex] + direction * delta;
		}

		public Vector3 GetSecondControlPoint()
		{
			float delta;
			Vector3 direction;

			if (_EndHandleIndex >= _ReceiverSpline.PointCount - 1)
			{
				delta = (_ReceiverSpline[ReceiverHandleIndex] - _ReceiverSpline[ReceiverHandleIndex - 1]).magnitude;
				direction = (_ReceiverSpline[ReceiverHandleIndex] - _ReceiverSpline[ReceiverHandleIndex - 1]).normalized;
				return _ReceiverSpline[ReceiverHandleIndex] + direction * delta;
			}

			delta = (_ReceiverSpline[ReceiverHandleIndex] - _ReceiverSpline[ReceiverHandleIndex + 1]).magnitude;
			direction = (_ReceiverSpline[ReceiverHandleIndex] - _ReceiverSpline[ReceiverHandleIndex + 1]).normalized;
			return _ReceiverSpline[ReceiverHandleIndex] + direction * delta;
		}
	}
}
