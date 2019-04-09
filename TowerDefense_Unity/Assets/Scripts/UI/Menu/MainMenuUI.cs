using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
	public class MainMenuUI : MonoBehaviour
	{
		private void Update()
		{
			//If someone either touches the screen or pressed the left mouse button, go the next scene
			if (Input.GetMouseButtonUp(0))
				Game.Utils.SceneUtility.LoadNext();
		}
	}
}

