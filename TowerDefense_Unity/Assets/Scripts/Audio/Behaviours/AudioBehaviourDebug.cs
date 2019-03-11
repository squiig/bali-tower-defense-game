using UnityEngine;

namespace Game.Audio.Debugging
{
	/// <summary>
	/// Used for debugging the audio system during runtime and real life scenarios.
	/// <para>NOTE: This behaviour should not be used in production!</para>
	/// </summary>
	public class AudioBehaviourDebug : MonoBehaviour
	{
		[SerializeField] private string _Identifier = "debug/debug";

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
				Audio.SendEvent(new AudioEvent(this, AudioCommands.PLAY, _Identifier));
		}
	}
}

