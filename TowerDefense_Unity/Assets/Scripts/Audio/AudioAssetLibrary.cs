using System;
using UnityEngine;

namespace Game.Audio
{
    /// <summary>
    /// DataClass: Contains mappings from indentifier to audio asset
    /// </summary>
    public class AudioAssetLibrary : ScriptableObject
    {
        [SerializeField] private AudioIndentifierMapping[] _AudioAssetIndentifierMappings;

        /// <summary>
        /// Resolves an indentifier to an audio asset
        /// </summary>
        public AudioAsset Resolve(string identifier)
        {
            foreach (AudioIndentifierMapping mapping in _AudioAssetIndentifierMappings)
            {
                if (mapping.Indentifier.Equals(identifier))
                {
                    return mapping.AudioAsset;
                }
            }

            throw new Exception($"Argument {identifier} was not found in Audiolibrary (name: {name})");
        }
    }
}