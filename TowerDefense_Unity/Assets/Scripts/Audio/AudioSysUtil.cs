using UnityEngine;

namespace Game.Audio
{
	public class AudioSysUtil
	{
		public static void ConfigureAudioSource(AudioSource audioSource, AudioAsset asset)
		{
			if (asset.ClipCount == 0)
			{
#if DEBUG
				Debug.LogWarning($"[Audio] tried playing asset {asset.name} but it has no clips.");
#endif
				return;
			}

			float pitch = asset.GetPitch();
			AudioClip clip = asset.GetClip();

			if (clip == null)
			{
#if DEBUG
				Debug.LogWarning($"[Audio] {asset.name} asset returned null as clip.");
#endif
				return;
			}

			audioSource.outputAudioMixerGroup = asset.AudioMixerGroup;
			audioSource.pitch = pitch;
			audioSource.clip = clip;
			audioSource.volume = asset.Volume;
			audioSource.timeSamples = pitch < 0 ? (clip.samples -1) : 0; // reverse clip if we play backwards
		}
	}
}
