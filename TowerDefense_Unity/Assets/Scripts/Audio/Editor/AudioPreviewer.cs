using UnityEditor;
using UnityEngine;

namespace Game.Audio.Editor
{
	/// <summary>
	/// Can be used to preview Audio Assets by temporarily creating a audio source in the scene.
	/// <para>NOTE: User has to call Remove() manually when preview is not available.</para>
	/// </summary>
	public class AudioPreviewer
	{
		private AudioSource _AudioSource;

		public void Create()
		{
			if (_AudioSource)
				return;

			GameObject gameObject = new GameObject {
				hideFlags = HideFlags.HideAndDontSave
			};

			_AudioSource = gameObject.AddComponent<AudioSource>();
			AssemblyReloadEvents.beforeAssemblyReload += Remove;
		}

		public void Remove()
		{
			if (!_AudioSource)
				return;

			Object.DestroyImmediate(_AudioSource.gameObject);
			_AudioSource = null;

			AssemblyReloadEvents.beforeAssemblyReload -= Remove;
		}

		public void Play(AudioAsset audioAsset)
		{
			AudioSysUtil.ConfigureAudioSource(_AudioSource, audioAsset);
			_AudioSource.Play();
		}
	}
}
