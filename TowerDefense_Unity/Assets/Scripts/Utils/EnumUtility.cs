using System;
using System.Collections;
using System.Collections.Generic;

namespace Game.Utils
{
	/*
	 * We will never be dealing with enumerations larger than 64 bits, and a 64-bit operation will
	 * produce the correct results even for 8-bit enumeration types. Therefore, we help the compiler
	 * by writing the operations as 64-bit explicitly.
	 */

	public static class EnumUtility
	{
		public static long GetValue(Enum instance)
		{
			return Convert.ToInt64(instance);
		}

		/// <summary>
		/// Only returns true if the enum has the exact given flag. Combined flags will be seen as unique and thus not work!
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="flag"></param>
		/// <returns></returns>
		public static bool HasFlag(Enum instance, Enum flag)
		{
			Type typeA = instance.GetType();
			Type typeB = flag.GetType();

			if (!Enum.IsDefined(typeA, flag))
				throw new ArgumentException(string.Format("Enumeration type mismatch. The flag is of type '{0}', was expecting '{1}'.", typeB, typeA));

			long a = GetValue(instance);
			long b = GetValue(flag);

			return (a & b) == b;
		}

		/// <summary>
		/// Only returns true if the enum has at least one flag of the given combination. (Combine with bitwise AND)
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="flagCombination"></param>
		/// <returns></returns>
		public static bool HasAnyFlagOfCombination(Enum instance, Enum flagCombination)
		{
			Type typeA = instance.GetType();
			Type typeB = flagCombination.GetType();

			if (!Enum.IsDefined(typeA, flagCombination))
				throw new ArgumentException(string.Format("Enumeration type mismatch. The flag is of type '{0}', was expecting '{1}'.", typeB, typeA));

			long a = GetValue(instance);
			long b = GetValue(flagCombination);

			return (a & b) != 0;
		}

		/// <summary>
		/// Only returns true if the enum has ALL of the given flags.
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="flags"></param>
		/// <returns></returns>
		public static bool HasFlags(Enum instance, params Enum[] flags)
		{
			if (flags.Length == 0)
				throw new ArgumentNullException("flags");

			for (int i = 0; i < flags.Length; i++)
			{
				if (!instance.HasFlag(flags[i]))
					return false;
			}

			return true;
		}
	}
}
