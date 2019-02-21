using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Game.Utils.Extensions
{
	public static class VectorExtensions
	{
		public static Vector3 MultipliedBy(this Vector3 a, Vector3 b)
		{
			return VectorUtility.Multiply(a, b);
		}

		public static Vector2 MultipliedBy(this Vector2 a, Vector2 b)
		{
			return VectorUtility.Multiply(a, b);
		}

		public static Vector3 NormalizedDifference(this Vector3 a, Vector3 b)
		{
			return VectorUtility.GetNormalizedDifference(a, b);
		}

		public static Vector2 NormalizedDifference(this Vector2 a, Vector2 b)
		{
			return VectorUtility.GetNormalizedDifference(a, b);
		}
	}
}
