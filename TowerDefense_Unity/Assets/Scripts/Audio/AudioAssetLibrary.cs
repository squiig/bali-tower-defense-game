using UnityEngine.Serialization;
using UnityEngine;
using System;

namespace Game.Audio
{
    /// <summary>
    /// DataClass: Contains mappings from indentifier to audio asset
    /// </summary>
    public class AudioAssetLibrary : ScriptableObject
    {
        [FormerlySerializedAs("AudioIndentifier")]
        [SerializeField] private AudioIdentifierMapping[] _AudioAssetIdentifierMappings;

        /// <summary>
        /// Resolves an indentifier to an audio asset
        /// </summary>
        public AudioAsset Resolve(string identifier)
        {
            foreach (AudioIdentifierMapping mapping in _AudioAssetIdentifierMappings)
            {
                if (mapping.Identifier.Equals(identifier))
                {
                    return mapping.AudioAsset;
                }
            }

            throw new Exception($"Argument {identifier} was not found in Audiolibrary (name: {name})");
        }
    }
}