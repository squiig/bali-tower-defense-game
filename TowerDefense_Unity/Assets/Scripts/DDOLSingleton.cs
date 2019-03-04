﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Game
{
	/// <summary>
	/// Same as the Singleton but with DontDestroyOnLoad functionality (persistency through scenes).
	/// </summary>
	/// <typeparam name="T">The class that's inheriting from DDOLSingleton.</typeparam>
	[DisallowMultipleComponent]
	public abstract class DDOLSingleton<T> : MonoBehaviour where T : Component
	{
		private static T _Instance;

		/// <summary>
		/// Referencing this field creates an instance if it doesn't already exist.
		/// </summary>
		public static T Instance
		{
			get {
				if (_Instance == null)
				{
					_Instance = FindObjectOfType<T>();
					if (_Instance == null)
					{
						GameObject obj = new GameObject();
						obj.name = typeof(T).Name;
						_Instance = obj.AddComponent<T>();
						DontDestroyOnLoad(obj);
					}
				}
				return _Instance;
			}
		}

		protected virtual void Awake()
		{
			if (_Instance == null)
			{
				_Instance = this as T;
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				Destroy(gameObject);
			}
		}
	}
}