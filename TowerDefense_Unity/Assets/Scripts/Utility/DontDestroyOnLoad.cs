using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Game
{
	public class DontDestroyOnLoad : MonoBehaviour
	{
		private void Start()
		{
			DontDestroyOnLoad(gameObject);
		}
	}
}