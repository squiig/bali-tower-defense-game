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
		private UnityCallbackBehaviour _Callbacks;
		private TransformFollower _TransformFollower;
		private int _ChannelNumber;

		public object Context { get; private set; } = null;
		public bool IsFree { get; private set; } = true;

		public AudioChannel(Transform parent, UnityCallbackBehaviour callbacks, int channelNumber = -1)
		{
			_Callbacks = callbacks;
			_ChannelNumber = channelNumber;
			_GameObject = new GameObject($"Audio Channel {channelNumber}");
			_TransformFollower = new TransformFollower(callbacks, _GameObject.transform);

			if (parent)
			{
				_GameObject.transform.SetParent(parent);
			}
			_GameObject.SetActive(false);

			_AudioSource = _GameObject.AddComponent<AudioSource>();
		}

		public void Update()
		{
			if (!IsFree && !_AudioSource.isPlaying)
			{
				_TransformFollower.StopFollowing();
				_Callbacks.OnUpdate -= Update;
				IsFree = true;
				_GameObject.name = $"Audio Channel {_ChannelNumber}";
			}
		}

		public void Play(AudioAsset asset, AudioEvent audioEvent)
		{
			IsFree = false;

			if (audioEvent.FollowTransform)
				_TransformFollower.StartFollowing(audioEvent.FollowTransform);
			_GameObject.transform.position = audioEvent.WorldPosition;

			_Callbacks.OnUpdate -= Update;
			_Callbacks.OnUpdate += Update;

			_GameObject.SetActive(true);
			Context = audioEvent.Context;
			AudioSysUtil.ConfigureAudioSource(_AudioSource, asset);
			_AudioSource.Play();
			_GameObject.name = $"Audio Channel {_ChannelNumber}: {audioEvent.Identifier}";
		}

		public void Stop()
		{
			if (_AudioSource)
				_AudioSource.Stop();
		}
	}
}
