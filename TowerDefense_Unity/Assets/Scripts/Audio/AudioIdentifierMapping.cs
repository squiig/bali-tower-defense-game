using UnityEngine;
using System;

namespace Game.Audio
{
    /// <summary>
    /// DataClass: Maps an indentifier to an audio asset
    /// </summary>
    [Serializable]
    public class AudioIdentifierMapping
    {
        [SerializeField] private string _Identifier;
        [SerializeField] private AudioAsset _AudioAsset;
        
        public string Identifier => _Identifier;
        public AudioAsset AudioAsset => _AudioAsset;
    }
}