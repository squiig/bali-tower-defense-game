using UnityEngine;

namespace Game.Audio
{
    /// <summary>
    /// Collection of audio players (channels)
    /// </summary>
    public class AudioChannelPool
    {
        private readonly AudioChannel[] m_AudioChannels;

        public AudioChannelPool(uint channelCount)
        {
            m_AudioChannels = new AudioChannel[channelCount];

            GameObject parentObject = new GameObject("Audio Channels");

            Object.DontDestroyOnLoad(parentObject);
            for (int i = 0; i < m_AudioChannels.Length; i++)
            {
                m_AudioChannels[i] = new AudioChannel(parentObject.transform);
            }
        }

        public void Play(AudioAsset asset, object context)
        {
            AudioChannel channel;
            
            if (FindFreeChannel(out channel))
            {
                channel.Play(asset, context);
            }
            else
            {
                Debug.LogWarningFormat("[Audio] All {0} channels(s) are full", m_AudioChannels.Length);
            }
        }

        public void StopContext(object context)
        {
            foreach (AudioChannel channel in m_AudioChannels)
            {
                if (channel.Context == context)
                {
                    channel.Stop();
                }
            }
        }

        private bool FindFreeChannel(out AudioChannel audioChannel)
        {
            foreach (AudioChannel channel in m_AudioChannels)
            {
                if (channel.IsFree)
                {
                    audioChannel = channel;
                    return true;
                }
            }

            audioChannel = null;
            return false;
        }
    }
}