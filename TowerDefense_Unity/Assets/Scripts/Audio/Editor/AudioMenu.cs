using Game.Utils;
using UnityEditor;
using Game.Utils.Editor;

namespace Game.Audio.Editor
{
    /// <summary>
    /// Contains menu items to open things from the audio editor
    /// </summary>
    public class AudioMenu
    {
        [MenuItem("Assets/Create/Audio/AudioAsset")]
        public static void CreateAudioAsset() => ScriptableObjectUtility.CreateAsset<AudioAsset>();

        [MenuItem("Assets/Create/Audio/AudioAssetLibrary")]
        public static void CreateAudioLibrary() => ScriptableObjectUtility.CreateAsset<AudioAssetLibrary>();

        [MenuItem("Window/AudioSystem")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<AudioSystemWindow>();
        }

        [MenuItem("Window/AudioAsset")]
        public static void ShowAudioAssetWindow()
        {
            EditorWindow.GetWindow<CreateAssetPopup<AudioAsset>>();
        }
    }
}