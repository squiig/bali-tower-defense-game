using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.SplineSystem
{
	/// <summary>Calculates all the different in-game paths and keeps track of them.
	/// Also returns the connections to make sure that the minions traverse the gaps in between the splines.</summary>
	public class SplinePathManager : MonoBehaviour
	{
		private Dictionary<BezierSplineDataObject, Vector3[]> _EvenlySpacedSplinePoints = new Dictionary<BezierSplineDataObject, Vector3[]>();

		public Vector3 this[BezierSplineDataObject dataObject, int i] => _EvenlySpacedSplinePoints[dataObject][i];

		private void Start()
		{
			
		}
	}
}
