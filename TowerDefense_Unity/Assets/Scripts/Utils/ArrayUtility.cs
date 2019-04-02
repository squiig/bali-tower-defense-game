using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils
{
	public static class ArrayUtility
	{
        /// <summary>
        /// Filters an array of GameObjects to Components
        /// </summary>
        public static T[] ComponentFilter<T>(IEnumerable<GameObject> gameObjects) where T : Component
        {
            List<T> filteredArray = new List<T>();
            foreach (GameObject gameObject in gameObjects)
            {
                T component = gameObject.GetComponent<T>();
                if (component)
                    filteredArray.Add(component);
            }
            return filteredArray.ToArray();
        }


		public static Vector2 GetAverageVec2(params Vector2[] _VelocitySamples)
		{
			Vector2 sum = Vector2.zero;
			foreach (var item in _VelocitySamples)
			{
				sum += item;
			}
			return sum / _VelocitySamples.Length;
		}


		public static float GetHighestValue(float[,] array)
		{
			float highest = .0f;

			int yLength = array.GetLength(1);
			for (int y = 0; y < yLength; ++y)
			{
				int xLength = array.GetLength(0);
				for (int x = 0; x < xLength; ++x)
				{
					if (array[x, y] > highest)
					{
						highest = array[x, y];
					}
				}
			}

			return highest;
		}

		public static float GetHighestValue(float[] array)
		{
			float highest = .0f;

			int length = array.Length;
			for (int i = 0; i < length; ++i)
			{
				if (array[i] > highest)
				{
					highest = array[i];
				}
			}

			return highest;
		}

		internal static float Average(float[] floats)
		{
			float sum = 0f;
			foreach (var item in floats)
			{
				sum += item;
			}
			return sum / floats.Length;
		}
	}
}
