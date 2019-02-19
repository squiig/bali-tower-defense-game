using System;
using UnityEditor;
using UnityEngine;

namespace Game.Audio.Editor
{
    public class AudioLibraryEditor
    {
        private readonly SelectableList _SelectableAudioAssetList;
        public event Action<AudioAsset> OnSelected;

        private AudioAssetLibrary _RawTarget;
        private SerializedObject _SerializedTarget;
        private SerializedProperty _MappingList;

        private Vector2 _ScrollVector;

        public AudioLibraryEditor()
        {
            _SelectableAudioAssetList = new SelectableList(DrawElement);
            _SelectableAudioAssetList.OnSelect += OnAudioAssetSelected;
        }

        public void SetTarget(AudioAssetLibrary library)
        {
            _SelectableAudioAssetList.ResetSelection();
            _RawTarget = library;
            _SerializedTarget = new SerializedObject(library);
            _MappingList = _SerializedTarget.FindProperty("_AudioAssetIndentifierMappings");
        }

        public void DoLibraryEditor()
        {
            if (_RawTarget == null)
            {
                return;
            }

            _ScrollVector = GUILayout.BeginScrollView(_ScrollVector);
            GUILayout.Label($"{nameof(AudioLibraryEditor)}");
            _SelectableAudioAssetList.DoList(_MappingList.arraySize, _ScrollVector);
            _SerializedTarget.ApplyModifiedProperties();
            GUILayout.EndScrollView();
        }

        private void DrawElement(int index)
        {
            SerializedProperty currentMapping = _MappingList.GetArrayElementAtIndex(index);
            SerializedProperty audioAssetProperty = currentMapping.FindPropertyRelative("_AudioAsset");
            SerializedProperty audioIdentifierProperty = currentMapping.FindPropertyRelative("_Identifier");

            audioIdentifierProperty.stringValue = EditorGUILayout.TextField(audioIdentifierProperty.stringValue);
            EditorGUILayout.ObjectField(audioAssetProperty);
        }

        private void OnAudioAssetSelected(int index)
        {
            if (index == -1)
            {
                OnSelected?.Invoke(null);
                return;
            }

            SerializedProperty currentMapping = _MappingList.GetArrayElementAtIndex(index);
            SerializedProperty audioAssetProperty = currentMapping.FindPropertyRelative("_AudioAsset");

            if (audioAssetProperty.objectReferenceValue == null)
            {
                OnSelected?.Invoke(null);
                return;
            }

            if (!AssetDatabase.TryGetGUIDAndLocalFileIdentifier(audioAssetProperty.objectReferenceValue, out string guid, out long _))
            {
                throw new Exception($"Couldn't fetch GUID of {audioAssetProperty.objectReferenceValue.name}, please make sure it is an scriptable object.");
            }

            string path = AssetDatabase.GUIDToAssetPath(guid);
            AudioAsset audioAsset = AssetDatabase.LoadAssetAtPath<AudioAsset>(path);

            OnSelected?.Invoke(audioAsset);
        }
    }
}
