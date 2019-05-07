using System;
using System.Collections;
using System.Collections.Generic;

namespace Game.Utils.Extensions
{
	public static class EnumExtensions
	{
		public static long GetValue(this Enum instance)
		{
			return EnumUtility.GetValue(instance);
		}

		/// <summary>
		/// Only returns true if the enum has the exact given flag. Combined flags will be seen as unique and thus not work!
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="flag"></param>
		/// <returns></returns>
		public static bool HasFlag(this Enum instance, Enum flag)
		{
			return EnumUtility.HasFlag(instance, flag);
		}

		/// <summary>
		/// Only returns true if the enum has at least one flag of the given combination. (Combine with bitwise AND)
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="flagCombination"></param>
		/// <returns></returns>
		public static bool HasAnyFlagOfCombination(this Enum instance, Enum flagCombination)
		{
			return EnumUtility.HasAnyFlagOfCombination(instance, flagCombination);
		}

		/// <summary>
		/// Only returns true if the enum has ALL of the given flags.
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="flags"></param>
		/// <returns></returns>
		public static bool HasFlags(this Enum instance, params Enum[] flags)
		{
			return EnumUtility.HasFlags(instance, flags);
		}
	}
}
