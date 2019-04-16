using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Game
{
	/// <summary>
	/// Non-persistent singleton class. Will make a new instance when called and none exists. Will destroy itself if another instance already exists. No thread-safety features.
	/// </summary>
	/// <typeparam name="T">The class that's inheriting from <see cref="MonoBehaviourSingleton{T}"/>.</typeparam>
	[DisallowMultipleComponent]
	public abstract class MonoBehaviourSingleton<T> : MonoBehaviourSingletonBase where T : Component
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

					if (_Instance == null && IsNewInstanceAllowed)
					{
						GameObject obj = new GameObject();
						obj.name = typeof(T).Name;
						_Instance = obj.AddComponent<T>();
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
			}
			else
			{
				if(_Instance != this as T)
					Destroy(gameObject);
			}
		}
	}
}
