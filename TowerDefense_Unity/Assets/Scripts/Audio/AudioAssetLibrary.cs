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
        /// Resolves an indentifier to an audio asset.
        /// </summary>
        /// <Returns> Audioasset or null on fail</Returns>
        public AudioAsset Resolve(string identifier)
        {
            foreach (AudioIdentifierMapping mapping in _AudioAssetIdentifierMappings)
            {
                if (mapping.Identifier.Equals(identifier))
                {
                    return mapping.AudioAsset;
                }
            }
			return null;
        }
    }
}
