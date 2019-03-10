using Game.Utils.Editor;
using UnityEditor;
using UnityEngine;

namespace Game.Audio.Editor
{
    /// <summary>
    /// Used to render and control a window to edit an audio asset
    /// </summary>
    public class AudioAssetEditor
    {
	    private const float PITCH_MINIMUM = -2f;
	    private const float PITCH_MAXIMUM = 2f;
	    private readonly SelectableList _SelectableList;

        private AudioAsset _RawTarget;
        private SerializedObject _SerializedTarget;
        private SerializedProperty _ClipList;

        private Vector3 _ScrollVector;

        public void SetTarget(AudioAsset audioAsset)
        {
            if (audioAsset == null)
            {
                ResetTarget();
                return;
            }

            _RawTarget = audioAsset;
            _SerializedTarget = new SerializedObject(_RawTarget);
            _ClipList = _SerializedTarget.FindProperty("_AudioClips");
            _SelectableList.ResetSelection();
        }

        public void AddElement()
        {
            _ClipList.InsertArrayElementAtIndex(0);
        }

        public void RemoveElement()
        {
            _ClipList.DeleteArrayElementAtIndex(
                _SelectableList.SelectedElementIndex);
        }

        public void RemoveAllElements()
        {
            _ClipList.ClearArray();
        }

        public void DoAssetEditor()
        {
            if (_RawTarget == null) return;

            _ScrollVector = GUILayout.BeginScrollView(_ScrollVector);

            GUILayout.BeginHorizontal();
            GUILayout.Label($"{nameof(AudioAssetEditor)}");

            if (GUILayout.Button("Nuke"))
            {
                EditorWindow.CreateInstance<ConfirmActionPopup>()
                    .SetQuestion("You're about to remove all soundclips. Are you sure?")
                    .OnConfirm += RemoveAllElements;
            }

            if (GUILayout.Button("Remove"))
            {
                RemoveElement();
            }

            if (GUILayout.Button("Add"))
            {
                AddElement();
            }


            GUILayout.EndHorizontal();


            EditorScriptUtil.RangeSlider(
	            _SerializedTarget.FindProperty("_PitchMin"),
	            _SerializedTarget.FindProperty("_PitchMax"),
	            PITCH_MINIMUM,
	            PITCH_MAXIMUM);

			_SelectableList.DoList(_ClipList.arraySize, _ScrollVector);
            _SerializedTarget.ApplyModifiedProperties();



            GUILayout.EndScrollView();
        }

        public AudioAssetEditor()
        {
            _SelectableList = new SelectableList(DrawElement);
        }

        private void DrawElement(int index)
        {
            SerializedProperty clip = _ClipList.GetArrayElementAtIndex(index);
            EditorGUILayout.ObjectField(clip);
        }

        private void ResetTarget()
        {
            _RawTarget = null;
            _SerializedTarget = null;
            _ClipList = null;
        }
    }
}
