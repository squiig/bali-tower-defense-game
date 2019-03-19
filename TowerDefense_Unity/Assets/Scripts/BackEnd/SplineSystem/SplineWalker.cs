using Game.Utils;
using UnityEngine;

namespace Game.SplineSystem
{
	/// <summary>The spline walker object will walk over one of the spline branches until he reaches the end,
	/// then he will look what state he is supposed to execute.</summary>
	public class SplineWalker : Entities.Entity
	{
		[SerializeField] private const float UPDATE_NEXT_DESTINATION = 0.1f;
		[SerializeField] private float _MoveSpeed = 7.0f;
		[SerializeField] private float _RotationSpeed = 5.0f;

		private SplinePathManager _PathManager;
		private int _CurrentDestinationIndex;

		public BezierSplineDataObject SplineBranch { get; set; }

		private void Start()
		{
			_CurrentDestinationIndex = 0;
			if (FindObjectOfType<SplinePathManager>() == null)
				return;
			_PathManager = FindObjectOfType<SplinePathManager>();
		}

		private void Update()
		{
			if (_PathManager == null)
				return;
			if ((_PathManager[SplineBranch, _CurrentDestinationIndex] - transform.position).magnitude < UPDATE_NEXT_DESTINATION)
				UpdateSplineDestinationPoint();
			else
				MoveSplineWalker();
		}

		private void MoveSplineWalker()
		{
			if (_PathManager.EvenlySpacedSplinePointCount(SplineBranch) - 1 >= _CurrentDestinationIndex + 1)
			{
				transform.rotation = Quaternion.Slerp(
				a: transform.rotation,
				b: Quaternion.LookRotation(Bezier3DUtility.GetTangent(
					_PathManager[SplineBranch, _CurrentDestinationIndex + 1],
					_PathManager[SplineBranch, _CurrentDestinationIndex])),
				t: _RotationSpeed * Time.deltaTime);
			}

			transform.position = Vector3.MoveTowards(transform.position, _PathManager[SplineBranch, _CurrentDestinationIndex], _MoveSpeed * Time.deltaTime);
		}

		private void UpdateSplineDestinationPoint()
		{
			if (_PathManager.EvenlySpacedSplinePointCount(SplineBranch) - 1 < _CurrentDestinationIndex + 1)
				return;
			_CurrentDestinationIndex++;
		}
	}
}
