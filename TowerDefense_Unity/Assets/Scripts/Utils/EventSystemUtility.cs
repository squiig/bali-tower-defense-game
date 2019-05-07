using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Utils
{
	public static class EventSystemUtility
	{
		public static void SetSelectedGameObject(GameObject go)
		{
#if !(UNITY_IOS || UNITY_ANDROID)
			SetNoSelectedGameObject();
			EventSystem.current.SetSelectedGameObject(go);
#endif
		}

		public static void SetNoSelectedGameObject()
		{
			EventSystem.current.SetSelectedGameObject(null);
		}
	}
}
