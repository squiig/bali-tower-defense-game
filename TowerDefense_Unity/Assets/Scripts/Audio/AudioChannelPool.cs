using UnityEngine;

namespace Game.Audio
{
    /// <summary>
    /// Collection of audio players (channels)
    /// </summary>
    public class AudioChannelPool
    {
        private readonly AudioChannel[] _AudioChannels;

        public AudioChannelPool(uint channelCount)
        {
            _AudioChannels = new AudioChannel[channelCount];

            GameObject parentObject = new GameObject("Audio Channels");

            Object.DontDestroyOnLoad(parentObject);
            for (int i = 0; i < _AudioChannels.Length; i++)
            {
                _AudioChannels[i] = new AudioChannel(parentObject.transform);
            }
        }

        public void Play(AudioAsset asset, object context)
        {
            if (FindFreeChannel(out AudioChannel channel))
            {
                channel.Play(asset, context);
            }
            else
            {
                Debug.LogWarningFormat("[Audio] All {0} channels(s) are full", _AudioChannels.Length);
            }
        }

        public void StopContext(object context)
        {
            foreach (AudioChannel channel in _AudioChannels)
                if (channel.Context == context) channel.Stop();
        }

        private bool FindFreeChannel(out AudioChannel audioChannel)
        {
            foreach (AudioChannel channel in _AudioChannels)
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