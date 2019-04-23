using UnityEngine;

namespace Game.Audio.Debugging
{
	public class LoopedAudioPlayer : MonoBehaviour
	{
		[SerializeField] private string _Identifier;

		private void OnEnable()
		{
			Audio.SendEvent(new AudioEvent(this, AudioCommands.PLAY, _Identifier));
		}

		private void OnDisable()
		{
			Audio.SendEvent(new AudioEvent(this, AudioCommands.STOP_CONTEXT));
		}
	}

}

