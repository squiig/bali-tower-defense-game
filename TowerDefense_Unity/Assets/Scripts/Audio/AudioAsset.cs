using UnityEngine;

namespace Game.Audio
{
    /// <summary>
    /// Dataclass: Contains audio clips and information about how these should be played
    /// </summary>
    public class AudioAsset : ScriptableObject
    {
        [SerializeField] private AudioClip[] _AudioClips;
        [SerializeField] private float _Volume = 1;

        public AudioClip[] AudioClips => _AudioClips;
        public int ClipCount => _AudioClips.Length;
        public float Volume => _Volume;

        public AudioClip GetClip()
        {
            return AudioClips[Random.Range(0, ClipCount)];
        }
    }
}
