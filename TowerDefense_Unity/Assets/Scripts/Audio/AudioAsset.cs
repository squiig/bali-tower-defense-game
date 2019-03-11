#pragma warning disable 649
using UnityEngine;
using UnityEngine.Audio;

namespace Game.Audio
{
    /// <summary>
    /// Dataclass: Contains audio clips and information about how these should be played
    /// </summary>
    public class AudioAsset : ScriptableObject
    {
		private const int INVALID_INDEX = -1;

		[SerializeField] private AudioMixerGroup _AudioMixerGroup;
        [SerializeField] private AudioClip[] _AudioClips;
        [SerializeField] private float _Volume = 1;

        [SerializeField] private float _PitchMin = 1;
        [SerializeField] private float _PitchMax = 1;

		[SerializeField] private bool _AvoidRepetition = true;
        private int _LastIndex = INVALID_INDEX;

        public AudioMixerGroup AudioMixerGroup => _AudioMixerGroup;
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
	        int index;

	        do
	        {
				index = Random.Range(0, ClipCount);
	        } while (index == _LastIndex && _AvoidRepetition && ClipCount > 1);

	        _LastIndex = index;
            return AudioClips[index];
        }
    }
}
