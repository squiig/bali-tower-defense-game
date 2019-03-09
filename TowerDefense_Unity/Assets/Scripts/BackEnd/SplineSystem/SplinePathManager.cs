using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Game.SplineSystem
{
	/// <summary>Calculates all the different in-game paths and keeps track of them.
	/// Also returns the connections to make sure that the minions traverse the gaps in between the splines.</summary>
	public class SplinePathManager : MonoBehaviourSingleton<SplinePathManager>
	{
		[SerializeField] private float _Spacing = 0.1f, _Resolution = 1f;
		private Dictionary<BezierSplineDataObject, Vector3[]> _EvenlySpacedSplinePoints = new Dictionary<BezierSplineDataObject, Vector3[]>();

		public Vector3 this[BezierSplineDataObject dataObject, int i] => _EvenlySpacedSplinePoints[dataObject][i];

		private void Start()
		{
			foreach(var spline in _EvenlySpacedSplinePoints)
			{
				Vector3[] points = CalculateEvenlySpacedPoints(spline.Key, _Spacing, _Resolution);
				foreach (Vector3 point in points)
				{
					GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
					sphere.transform.position = point;
					sphere.transform.localScale = Vector3.one * _Spacing * 0.5f;
					sphere.transform.parent = transform;
				}
			}
		}

		public Vector3[] GetStartedPoints()
		{
			Vector3[] starterPoints = new Vector3[_EvenlySpacedSplinePoints.Count];
			int counter = 0;

			foreach (KeyValuePair<BezierSplineDataObject, Vector3[]> spline in _EvenlySpacedSplinePoints)
			{
				starterPoints[counter] = spline.Key[0];
				counter++;
			}

			return starterPoints;
		}

		public Vector3[] CalculateEvenlySpacedPoints(BezierSplineDataObject spline, float spacing, float resolution = 1)
		{
			float splineLength = 0;
			List<Vector3> points = GetEvenlySpacedPoints(spline, ref splineLength, spacing, resolution);
			float remainingPointLength = spline.IsClosed
				? Mathf.Sqrt(
					(points[0].x - points[points.Count - 1].x) * (points[0].x - points[points.Count - 1].x) +
					(points[0].y - points[points.Count - 1].y) * (points[0].y - points[points.Count - 1].y) +
					(points[0].z - points[points.Count - 1].z) * (points[0].z - points[points.Count - 1].z))
				: Mathf.Sqrt(
					(spline[spline.PointCount - 1].x - points[points.Count - 1].x) * (spline[spline.PointCount - 1].x - points[points.Count - 1].x) +
					(spline[spline.PointCount - 1].y - points[points.Count - 1].y) * (spline[spline.PointCount - 1].y - points[points.Count - 1].y) +
					(spline[spline.PointCount - 1].z - points[points.Count - 1].z) * (spline[spline.PointCount - 1].z - points[points.Count - 1].z));

			spacing += remainingPointLength / points.Count;
			return GetEvenlySpacedPoints(spline, ref splineLength, spacing, resolution).ToArray();
		}

		private List<Vector3> GetEvenlySpacedPoints(BezierSplineDataObject spline, ref float splineLength, float spacing, float resolution = 1)
		{
			List<Vector3> points = new List<Vector3> { spline[0] };
			Vector3 prevPoint = spline[0];
			float distanceSinceLastPoint = 0;

			for (int segmentIndex = 0; segmentIndex < spline.SegmentCount; segmentIndex++)
			{
				Vector3[] segment = spline.GetSegmentPoints(segmentIndex);
				float curveLength = Utils.Bezier3DUtility.Get3DCurveLength(segment[0], segment[1], segment[2], segment[3]);
				int divisions = Mathf.CeilToInt(curveLength * resolution * 10);
				splineLength += curveLength;

				float pointPos = 0;
				while (pointPos <= 1)
				{
					pointPos += 1f / divisions;
					Vector3 pointOnCurve = Utils.Bezier3DUtility.CubicCurveVector3(segment[0], segment[1], segment[2], segment[3], pointPos);
					distanceSinceLastPoint += Vector3.Distance(prevPoint, pointOnCurve);

					while (distanceSinceLastPoint >= spacing)
					{
						float overShootDistance = distanceSinceLastPoint - spacing;
						Vector3 newCorrectedPoint = pointOnCurve + (prevPoint - pointOnCurve).normalized * overShootDistance;
						points.Add(newCorrectedPoint);
						distanceSinceLastPoint = overShootDistance;
						prevPoint = newCorrectedPoint;
					}
					prevPoint = pointOnCurve;
				}
			}
			return points;
		}
	}
}
