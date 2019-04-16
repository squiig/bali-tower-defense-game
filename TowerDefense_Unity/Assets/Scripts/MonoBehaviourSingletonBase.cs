using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	/// <summary>
	/// This seemingly unnecessary extra layer of inheritence is to ensure the <see cref="DisallowMultipleComponent"/> attribute to take effect on any singleton regardless of the generic type.
	/// </summary>
	[DisallowMultipleComponent]
	public abstract class MonoBehaviourSingletonBase : MonoBehaviour
	{
		private static bool _IsNewInstanceAllowed = true;

		public static bool IsNewInstanceAllowed => _IsNewInstanceAllowed;

		protected virtual void OnEnable()
		{
			_IsNewInstanceAllowed = true;
		}

		protected virtual void OnDisable()
		{
			_IsNewInstanceAllowed = false;
		}

		protected virtual void OnDestroy()
		{
			_IsNewInstanceAllowed = true;
		}
	}
}
