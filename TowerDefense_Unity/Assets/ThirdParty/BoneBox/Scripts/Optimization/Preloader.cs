using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoneBox.Optimization
{
	/// <summary>
	/// This runs the second scene in the build settings (index one) at Start.
	/// It's meant to be used in a preloader scene, which is an empty scene with build index zero.
	/// </summary>
	public class Preloader : MonoBehaviour
	{
		private void Start()
		{
			SceneManager.LoadScene(1);
		}
	}
}