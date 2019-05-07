using System.Collections;
using System.Collections.Generic;

namespace Game.Utils.Extensions
{
	public static class TextExtensions
	{
		public static char? FirstNonWhitespace(this string str)
		{
			return TextUtility.FirstNonWhitespace(str);
		}
	}
}
