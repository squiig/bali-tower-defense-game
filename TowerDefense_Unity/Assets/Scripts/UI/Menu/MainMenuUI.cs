using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.UI
{
	public class MainMenuUI : MonoBehaviour
	{
		private void Update()
		{
			//If someone either touches the screen or pressed the left mouse button, go the next scene
			if (Input.GetMouseButtonDown(0))
				//TODO: Ones there is a new Scene Loader, replace this line with that.
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
	}
}

