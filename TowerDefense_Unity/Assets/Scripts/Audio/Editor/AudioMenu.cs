using Game.Utils;
using UnityEditor;

namespace Game.Audio
{
    public class AudioMenu
    {
        [MenuItem("Assets/Create/Audio/AudioAsset")]
        public static void CreateAudioAsset() => ScriptableObjectUtility.CreateAsset<AudioAsset>();

        [MenuItem("Assets/Create/Audio/AudioAssetLibrary")]
        public static void CreateAudioLibrary() => ScriptableObjectUtility.CreateAsset<AudioAssetLibrary>();
    }
}