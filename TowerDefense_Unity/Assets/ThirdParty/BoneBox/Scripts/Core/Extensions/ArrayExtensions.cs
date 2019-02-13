using System.Collections;
using System.Collections.Generic;

namespace BoneBox.Core.Extensions
{
	public static class ArrayExtensions
	{
		public static float GetHighestValue(this float[,] array)
		{
			return ArrayUtility.GetHighestValue(array);
		}

		public static float GetHighestValue(this float[] array)
		{
			return ArrayUtility.GetHighestValue(array);
		}
	}
}
