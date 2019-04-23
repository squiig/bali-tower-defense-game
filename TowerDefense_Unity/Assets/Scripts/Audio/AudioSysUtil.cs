using UnityEngine;

namespace Game.Audio
{
	public static class AudioSysUtil
	{
		public static void ConfigureAudioSource(AudioSource audioSource, AudioAsset asset)
		{
			if (asset.ClipCount == 0)
			{
				AudioLog.Warning($"tried playing asset {asset.name} but it has no clips.");
				return;
			} 

			float pitch = asset.GetPitch();
			AudioClip clip = asset.GetClip();

			if (clip == null)
			{
				AudioLog.Warning($"{asset.name} asset returned null as clip.");
				return;
			}

			audioSource.spatialBlend = asset.SpatialBlend;
			audioSource.dopplerLevel = asset.Doppler;

			audioSource.loop = asset.IsLooped;
			audioSource.panStereo = asset.Pan;
			audioSource.outputAudioMixerGroup = asset.AudioMixerGroup;
			audioSource.pitch = pitch;
			audioSource.clip = clip;
			audioSource.volume = asset.Volume;
			audioSource.maxDistance = asset.MaxDistance;
			audioSource.minDistance = asset.MinDistance;

			if (asset.RandomStartPosition)
				audioSource.timeSamples = Random.Range(0, clip.samples-1);
			else
				audioSource.timeSamples = pitch < 0 ? (clip.samples - 1) : 0;

		}
	}
}
