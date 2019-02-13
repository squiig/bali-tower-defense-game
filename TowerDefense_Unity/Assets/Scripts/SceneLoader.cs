using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using USceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Game
{
	public class SceneLoader : MonoBehaviour
	{
#if UNITY_EDITOR || DEBUG_ENABLED
		[SerializeField] private KeyCode m_ReloadKey = KeyCode.R;

		private void Update()
		{
			if (Input.GetKeyUp(m_ReloadKey))
			{
				ReloadActive();
			}
		}
#endif

		public static void LoadScene(string name)
		{
			USceneManager.LoadScene(name);
		}

		public void InstanceLoadScene(string sceneName)
		{
			LoadScene(sceneName);
		}

		public static void LoadScene(int buildIndex)
		{
			USceneManager.LoadScene(buildIndex);
		}

		public void InstanceLoadScene(int buildIndex)
		{
			LoadScene(buildIndex);
		}

		public static void ReloadActive()
		{
			USceneManager.LoadScene(USceneManager.GetActiveScene().buildIndex);
		}
	}
}