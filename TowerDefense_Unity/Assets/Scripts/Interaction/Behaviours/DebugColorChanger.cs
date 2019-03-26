using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Interaction.Debugging
{
	public class DebugColorChanger : MonoBehaviour, ITappable
	{
		public void Tapped()
		{
			GetComponent<Renderer>().material.color = Random.ColorHSV(0,1,0.4f,1);
		}
	}
}
