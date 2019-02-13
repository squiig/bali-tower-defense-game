using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BoneBox.Audio
{
    using Data;
    using Debugging;

    public class SFXPlayer : MonoBehaviour
    {
        public const float DEFAULT_VOLUME = 0.5f;
        public const float DEFAULT_MIN_PITCH = 0.95f;
        public const float DEFAULT_MAX_PITCH = 1.05f;

        public readonly SFXPlaySettings DEFAULT_PLAY_SETTINGS = new SFXPlaySettings(DEFAULT_VOLUME, new MinMaxFloat(DEFAULT_MIN_PITCH, DEFAULT_MAX_PITCH), false);

        [SerializeField] protected AudioClip[] m_PredefinedClips;
        [SerializeField] protected MinMaxInt m_SourceBufferCountRange = new MinMaxInt(3, 10);
        [SerializeField] private bool m_UseSourceBufferForPredefs = true;

        public MinMaxInt SourceBufferCountRange => m_SourceBufferCountRange;

        private AudioSource[] m_SourceBuffers;

        public AudioSource[] SourceBuffers
        {
            get {
                return m_SourceBuffers;
            }
            protected set {
                m_SourceBuffers = value;
            }
        }

        protected virtual void Start()
        {
            CreateFirstSourceBuffers();
        }

        #region Private Functions

        /// <summary>
        /// Initializes the full buffer, but adds only the minimum amount of audio source components to gameObject, defined by the count range.
        /// </summary>
        private void CreateFirstSourceBuffers()
        {
            m_SourceBuffers = new AudioSource[m_SourceBufferCountRange.Max];

            for (int i = 0; i < m_SourceBufferCountRange.Min; i++)
            {
                m_SourceBuffers[i] = gameObject.AddComponent<AudioSource>();
            }
        }

        /// <summary>
        /// Returns the first inactive audio source it can find, otherwise returns the first one.
        /// </summary>
        private AudioSource GetInactiveAudioSource()
        {
            AudioSource result = m_SourceBuffers[0];

            for (int i = 0; i < m_SourceBuffers.Length; i++)
            {
                AudioSource source = m_SourceBuffers[i];

                if (source != null)
                {
                    if (!source.isPlaying)
                    {
                        result = source;
                    }
                }
            }

            Logger.LogWarning(this, "Cannot find inactive audio source! Using first in list. Perhaps increase the max buffer count?");
            return result;
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Play an audio clip with a specific source and settings.
        /// </summary>
        /// <param name="clip">The audio clip to play.</param>
        /// <param name="source">The audio source to play the clip with.</param>
        /// <param name="settings">The playback settings to use.</param>
        public void Play(AudioClip clip, AudioSource source, SFXPlaySettings settings)
        {
            source.clip = clip;
            source.pitch = settings.PitchRange.GetRandom();
            source.volume = settings.Volume;
            source.loop = settings.Loop;
            source.Play();
        }

        /// <summary>
        /// Play an audio clip with default settings.
        /// </summary>
        /// <param name="clip">The audio clip to play.</param>
        /// <param name="source">The audio source to play the clip with.</param>
        public void Play(AudioClip clip, AudioSource source)
        {
            Play(clip, source, DEFAULT_PLAY_SETTINGS);
        }

        /// <summary>
        /// Play an audio clip with default settings through dynamic buffering. (if enabled)
        /// </summary>
        /// <param name="clip">The audio clip to play.</param>
        public void Play(AudioClip clip)
        {
            AudioSource source = m_UseSourceBufferForPredefs ? GetInactiveAudioSource() : m_SourceBuffers[0];
            Play(clip, source, DEFAULT_PLAY_SETTINGS);
        }

        /// <summary>
        /// Play an audio clip through dynamic buffering.
        /// </summary>
        /// <param name="clip">The audio clip to play.</param>
        /// <param name="settings">The playback settings to use.</param>
        public void Play(AudioClip clip, SFXPlaySettings settings)
        {
            AudioSource source = m_UseSourceBufferForPredefs ? GetInactiveAudioSource() : m_SourceBuffers[0];
            Play(clip, source, settings);
        }

        /// <summary>
        /// Play a random audio clip with a specific source and settings.
        /// </summary>
        /// <param name="clip">The audio clip to play.</param>
        /// <param name="source">The audio source to play the clip with.</param>
        /// <param name="settings">The playback settings to use.</param>
        public void PlayRandom(AudioClip[] clips, AudioSource source, SFXPlaySettings settings)
        {
            source.clip = clips[Random.Range(0, clips.Length)];
            source.pitch = settings.PitchRange.GetRandom();
            source.volume = settings.Volume;
            source.loop = settings.Loop;
            source.Play();
        }

        /// <summary>
        /// Play a random audio clip with default settings.
        /// </summary>
        /// <param name="clip">The audio clip to play.</param>
        /// <param name="source">The audio source to play the clip with.</param>
        public void PlayRandom(AudioClip[] clips, AudioSource source)
        {
            Play(clips[Random.Range(0, clips.Length)], source, DEFAULT_PLAY_SETTINGS);
        }

        /// <summary>
        /// Play a random audio clip with default settings through dynamic buffering.
        /// </summary>
        /// <param name="clip">The audio clip to play.</param>
        public void PlayRandom(AudioClip[] clips)
        {
            AudioSource source = GetInactiveAudioSource();
            Play(clips[Random.Range(0, clips.Length)], source);
        }

        /// <summary>
        /// Play a random audio clip from the predefined list with default settings through dynamic buffering.
        /// </summary>
        public void PlayRandom()
        {
            AudioSource source = m_UseSourceBufferForPredefs ? GetInactiveAudioSource() : m_SourceBuffers[0];
            PlayRandom(m_PredefinedClips, source);
        }

        /// <summary>
        /// Play a random audio clip through dynamic buffering.
        /// </summary>
        /// <param name="clip">The audio clip to play.</param>
        /// <param name="settings">The playback settings to use.</param>
        public void PlayRandom(AudioClip[] clips, SFXPlaySettings settings)
        {
            AudioSource source = GetInactiveAudioSource();
            Play(clips[Random.Range(0, clips.Length)], source, settings);
        }

        #endregion
    }
}