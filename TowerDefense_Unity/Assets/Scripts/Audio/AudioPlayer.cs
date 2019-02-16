using UnityEngine;

namespace Game.Audio
{
    /// <summary>
    /// Wrapper around the object that plays a sound
    /// </summary>
    public class AudioChannel
    {
        private GameObject m_GameObject;
        private AudioSource m_AudioSource;

        public object Context { get; private set; } = null;

        public bool IsFree => !m_AudioSource.isPlaying;

        public AudioChannel(Transform parent = null, int channelNumber = -1)
        {
            m_GameObject = new GameObject("Audio Channel {0}");

            if (parent)
            {
                m_GameObject.transform.SetParent(parent);
            }
            m_GameObject.SetActive(false);

            m_AudioSource = m_GameObject.AddComponent<AudioSource>();
        }

        public void Play(AudioAsset asset, object context)
        {
            m_GameObject.transform.SetAsFirstSibling();
            m_GameObject.SetActive(true);
            Context = context;
            m_AudioSource.clip = asset.GetClip();
            m_AudioSource.volume = asset.Volume;
            m_AudioSource.Play();
        }

        public void Stop()
        {
            m_AudioSource.Stop();
        }
    }
}