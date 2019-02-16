using UnityEngine;
using System;

namespace Game.Audio
{
    /// <summary>
    /// DataClass: Maps an indentifier to an audio asset
    /// </summary>
    [Serializable]
    public class AudioIndentifierMapping
    {
        [SerializeField] private string m_Identifier;
        [SerializeField] private AudioAsset m_AudioAsset;

        public string Indentifier => m_Identifier;
        public AudioAsset AudioAsset => m_AudioAsset;
    }
}