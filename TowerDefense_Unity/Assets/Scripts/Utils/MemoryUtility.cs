using System.Collections;
using System.Collections.Generic;

namespace Game.Utils
{
	public static class MemoryUtility
	{
		/// <summary>
		/// Switches a variable between two values. If the current value is neither A or B, it defaults to A.
		/// </summary>
		/// <typeparam name="T">The type of the variables.</typeparam>
		/// <param name="value">The variable to switch.</param>
		/// <param name="a">Option A.</param>
		/// <param name="b">Option B.</param>
		public static void Switch<T>(ref T value, T a, T b)
		{
			value = (value.Equals(a)) ? b : a;
		}

		/// <summary>
		/// Swaps two variables' values with each other.
		/// </summary>
		/// <typeparam name="T">The type of the variables.</typeparam>
		/// <param name="a">Variable A, which will get B's value.</param>
		/// <param name="b">Variable B, which will get A's value.</param>
		public static void Swap<T>(ref T a, ref T b)
		{
			T temp = a;
			a = b;
			b = temp;
		}
	}
}
