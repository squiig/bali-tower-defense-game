using System.Collections;
using System.Collections.Generic;

namespace BoneBox.Core
{
	public static class TextUtility
	{
		public static char? FirstNonWhitespace(string str)
		{
			int length = str.Length;
			for (int i = 0; i < length; i++)
			{
				if (!char.IsWhiteSpace(str[i]))
				{
					return str[i];
				}
			}

			return null;
		}
	}
}
