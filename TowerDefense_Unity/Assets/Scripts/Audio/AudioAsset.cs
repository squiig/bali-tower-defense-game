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

        [SerializeField] private float _PitchMin = 1;
        [SerializeField] private float _PitchMax = 1;

        public AudioClip[] AudioClips => _AudioClips;
        public int ClipCount => _AudioClips.Length;
        public float Volume => _Volume;
        public float Pitch => GetPitch();

        public float GetPitch()
        {
	        return Random.Range(_PitchMin, _PitchMax);
        }

        public AudioClip GetClip()
        {
            return GetRandomClip(); // temp
        }

        private AudioClip GetRandomClip()
        {
            return AudioClips[Random.Range(0, ClipCount)];
        }
    }
}
