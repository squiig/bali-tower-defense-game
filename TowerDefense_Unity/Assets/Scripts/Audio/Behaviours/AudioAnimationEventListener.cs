using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Audio.Behaviours
{
	public class AudioAnimationEventListener : MonoBehaviour
	{
		// Should be spoken to by 
		public void AudioEvent(string identifier)
		{
			Audio.SendEvent(new AudioEvent(this, AudioCommands.PLAY, identifier, followTransform:transform));
		}
	}
}
