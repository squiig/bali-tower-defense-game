using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
	public class CanvasCameraProvider : MonoBehaviour
	{
		void Start()
		{
			if (TryGet(out Canvas canvas))
			{
				canvas.worldCamera = Camera.main;
			}
		}

		private bool TryGet<T>(out T component) where T : Component
		{
			return component = GetComponent<T>();
		}
	}
}
