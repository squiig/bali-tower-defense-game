using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BoneBox.SceneManagement
{
	public class DontDestroyOnLoad : MonoBehaviour
	{
		private void Start()
		{
			DontDestroyOnLoad(gameObject);
		}
	}
}