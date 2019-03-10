using UnityEngine;

namespace Game.Audio
{
	/// <summary>
	/// Wrapper around the object that plays a sound
	/// </summary>
	public class AudioChannel
	{
		private GameObject _GameObject;
		private AudioSource _AudioSource;

		public object Context { get; private set; } = null;

		public bool IsFree => !_AudioSource.isPlaying;

		public AudioChannel(Transform parent = null, int channelNumber = -1)
		{
			_GameObject = new GameObject($"Audio Channel {channelNumber}");

			if (parent)
			{
				_GameObject.transform.SetParent(parent);
			}
			_GameObject.SetActive(false);

			_AudioSource = _GameObject.AddComponent<AudioSource>();
		}

		public void Play(AudioAsset asset, object context)
		{
#if UNITY_EDITOR // for debugging purposes
			_GameObject.transform.SetAsFirstSibling();
#endif
			_GameObject.SetActive(true);
			Context = context;

			AudioSysUtil.ConfigureAudioSource(_AudioSource, asset);
			_AudioSource.Play();
		}

		public void Stop()
		{
			_AudioSource.Stop();
		}
	}
}
