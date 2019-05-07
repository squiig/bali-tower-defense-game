using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class Singleton<T>
	{
		private static readonly Lazy<Singleton<T>> _Instance = new Lazy<Singleton<T>>(() => new Singleton<T>());

		public static Singleton<T> Instance => _Instance.Value;

		/*
		 * Explicit static constructor to tell C# compiler 
		 * not to mark type as beforefieldinit.
		 */
		static Singleton()
		{
		}

		private Singleton()
		{
		}
	}
}
