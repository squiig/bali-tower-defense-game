using UnityEngine;

namespace Game.Audio
{
    /// <summary>
    /// Basically processes audio events and sends necessary information all over the place.
    /// </summary>
    public class AudioManager
    {
        private const int AUDIO_CHANNEL_COUNT = 50;
        private AudioAssetLibrary[] _Libraries;
        private AudioChannelPool _AudioChannelPool;

        public AudioManager()
        {
            _AudioChannelPool = new AudioChannelPool(AUDIO_CHANNEL_COUNT);
            _Libraries = FindAudioLibraries();

            AudioLog.Assert(_Libraries.Length > 0, "No audiolibraries found in root resources folder");
        }

        private AudioAssetLibrary[] FindAudioLibraries()
        {
            // Magically fish an AudioLibrary out of the resources.
            // I wish here was more dynamic functionality for this but Unity3D.
            return Resources.LoadAll<AudioAssetLibrary>("");
        }

        public void ProcessEvent(AudioEvent audioParams)
        {
            switch (audioParams.AudioCommand)
            {
                case AudioCommands.PLAY:
                    HandlePlayEvent(audioParams);
                    break;
                case AudioCommands.STOP_CONTEXT:
                    HandleStopEvent(audioParams);
                    break;
                default:
                    return;
            }
        }

        private void HandlePlayEvent(AudioEvent audioEvent)
        {
	        AudioAsset asset = ResolveAsset(audioEvent);

			if (!asset)
				return;

			_AudioChannelPool.Play(asset, audioEvent);
        }

        private AudioAsset ResolveAsset(AudioEvent audioEvent)
        {
			AudioAsset asset = null;
			for (int i = 0; i < _Libraries.Length; i++)
	        {
				asset = _Libraries[i].Resolve(audioEvent.Identifier);
				if (asset)
					break;
	        }

			AudioLog.Assert(asset, $"{audioEvent.Identifier} could not be found in any audio library.");
			return asset;
        }

        private void HandleStopEvent(AudioEvent audioEvent)
        {
            _AudioChannelPool.StopContext(audioEvent.Context);
        }
    }
}
