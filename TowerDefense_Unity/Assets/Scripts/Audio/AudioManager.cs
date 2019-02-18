using UnityEngine;

namespace Game.Audio
{
    /// <summary>
    /// Basically processes audio events and sends necessary information all over the place.
    /// </summary>
    public class AudioManager
    {
        private const int AUDIO_CHANNEL_COUNT = 20;
        private AudioAssetLibrary m_Library;
        private AudioChannelPool m_AudioChannelPool;

        public AudioManager()
        {

            m_AudioChannelPool = new AudioChannelPool(AUDIO_CHANNEL_COUNT);
            m_Library = FindAudioLibrary();
            
            if (!m_Library)
            {
                Debug.LogError("[Audio] AudioLibrary not found in resources, make sure it's in the root");
            }
        }

        private AudioAssetLibrary FindAudioLibrary()
        {
            // Magically fish an AudioLibrary out of the resources.
            // I wish here was more dynamic functionality for this but Unity3D.
            return Resources.Load<AudioAssetLibrary>("AudioAssetLibrary");
        }

        public void ProcessEvent(AudioEvent audioParams)
        {
            switch (audioParams.Command)
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
            m_AudioChannelPool.Play(
                m_Library.Resolve(audioEvent.Identifier),
                audioEvent.Context);
        }

        private void HandleStopEvent(AudioEvent audioEvent)
        {
            m_AudioChannelPool.StopContext(audioEvent.Context);
        }
    }
}