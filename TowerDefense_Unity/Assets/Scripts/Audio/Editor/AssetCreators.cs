using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Audio.Editor
{
    /// <summary>
    /// Used to give a name to the audio asset before we create it
    /// </summary>
    public class CreateAudioAssetPopup : CreateAssetPopup<AudioAsset>
    {
        // Class exists because unity doesn't support
        // generic typing in EditorWindow.CreateInstance
    }

    /// <summary>
    /// Used to give a name to the audio asset library before we create it
    /// </summary>
    public class CreateAudioLibraryAssetPopup : CreateAssetPopup<AudioAssetLibrary>
    {
        // Class exists because unity doesn't support
        // generic typing in EditorWindow.CreateInstance
    }
}

