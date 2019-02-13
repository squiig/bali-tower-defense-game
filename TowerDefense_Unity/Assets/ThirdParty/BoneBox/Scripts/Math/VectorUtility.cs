using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BoneBox.Math
{
	public static class VectorUtility
	{
		public static Vector3 GetAveragePosition(Component[] components)
		{
			Vector3 sum = Vector3.zero;

			int length = components.Length;
			for (int i = 0; i < length; ++i)
			{
				sum += components[i].transform.position;
			}

			return sum / length;
		}

		public static Vector3 GetAveragePosition(GameObject[] gameObjects)
		{
			Vector3 sum = Vector3.zero;

			int length = gameObjects.Length;
			for (int i = 0; i < length; ++i)
			{
				sum += gameObjects[i].transform.position;
			}

			return sum / length;
		}

		public static Vector3 Multiply(Vector3 a, Vector3 b)
		{
			return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
		}

		public static Vector2 Multiply(Vector2 a, Vector2 b)
		{
			return Multiply((Vector3)a, (Vector3)b);
		}

		public static Vector3 GetNormalizedDifference(Vector3 a, Vector3 b)
		{
			Vector3 c = a - b;
			float length = c.magnitude;

			if (length > 0.0f)
			{
				c.x /= length;
				c.y /= length;
				c.z /= length;
			}

			return c;
		}

		public static Vector2 GetNormalizedDifference(Vector2 a, Vector2 b)
		{
			return GetNormalizedDifference((Vector3)a, (Vector3)b);
		}
	}
}
