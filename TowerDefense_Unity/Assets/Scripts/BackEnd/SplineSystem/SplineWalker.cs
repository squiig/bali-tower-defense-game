using Game.Utils;
using UnityEngine;

namespace Game.SplineSystem
{
	/// <summary>The spline walker object will walk over one of the spline branches until he reaches the end,
	/// then he will look what state he is supposed to execute.</summary>
	public class SplineWalker : Entities.Entity
	{
		[SerializeField] private const float UPDATE_NEXT_DESTINATION = 0.1f;
		[SerializeField] protected float _MoveSpeed = 7.0f;
		[SerializeField] protected float _RotationSpeed = 5.0f;

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

		protected virtual void Update()
		{
			if (_PathManager == null)
				return;
			Vector3 delta = _PathManager[SplineBranch, _CurrentDestinationIndex] - transform.position;
			if (new Vector3(delta.x, 0, delta.z).magnitude < UPDATE_NEXT_DESTINATION)
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

			transform.position = Vector3.MoveTowards(
				current: transform.position,
				target: new Vector3(
					x: _PathManager[SplineBranch, _CurrentDestinationIndex].x,
					y: transform.position.y,
					z: _PathManager[SplineBranch, _CurrentDestinationIndex].z),
				maxDistanceDelta: _MoveSpeed * Time.deltaTime);
		}

		private void UpdateSplineDestinationPoint()
		{
			if (_PathManager.EvenlySpacedSplinePointCount(SplineBranch) - 1 < _CurrentDestinationIndex + 1)
				return;
			_CurrentDestinationIndex++;
		}
	}
}
