using UnityEngine;

namespace Game.Audio
{
    /// <summary>
    /// Dataclass: Contains audio clips and information about how these should be played
    /// </summary>
    public class AudioAsset : ScriptableObject
    {
        [SerializeField] private AudioClip[] m_AudioClips;
        [SerializeField] private float m_Volume = 1;

        public AudioClip[] AudioClips => m_AudioClips;
        public int ClipCount => m_AudioClips.Length;
        public float Volume => m_Volume;

        public AudioClip GetClip()
        {
            return AudioClips[Random.Range(0, ClipCount)];
        }
    }
}
