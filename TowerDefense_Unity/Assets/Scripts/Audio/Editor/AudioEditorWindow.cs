using System;
using UnityEditor;
using UnityEngine;

namespace Game.Audio.Editor
{
    public class AudioLibraryEditor
    {
        private SelectableList m_SelectableAudioAssetList;
        public event Action<AudioAsset> OnSelected;

        private AudioAssetLibrary m_RawTarget;
        private SerializedObject m_SerializedTarget;
        private SerializedProperty m_MappingList;

        private Vector2 m_ScrollVector;

        public AudioLibraryEditor()
        {
            m_SelectableAudioAssetList = new SelectableList(DrawElement);
            m_SelectableAudioAssetList.OnSelect += OnAudioAssetSelected;
        }

        public void SetTarget(AudioAssetLibrary library)
        {
            m_SelectableAudioAssetList.ResetSelection();
            m_RawTarget = library;
            m_SerializedTarget = new SerializedObject(library);
            m_MappingList = m_SerializedTarget.FindProperty("m_AudioAssetIndentifierMappings");
        }

        public void DoLibraryEditor()
        {
            if (m_RawTarget == null)
            {
                return;
            }

            m_ScrollVector = GUILayout.BeginScrollView(m_ScrollVector);
            GUILayout.Label($"{nameof(AudioLibraryEditor)}");
            m_SelectableAudioAssetList.DoList(m_MappingList.arraySize, m_ScrollVector);
            m_SerializedTarget.ApplyModifiedProperties();
            GUILayout.EndScrollView();
        }

        private void DrawElement(int index)
        {
            SerializedProperty currentMapping = m_MappingList.GetArrayElementAtIndex(index);
            SerializedProperty audioAssetProperty = currentMapping.FindPropertyRelative("m_AudioAsset");
            SerializedProperty audioIdentifierProperty = currentMapping.FindPropertyRelative("m_Identifier");

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

            SerializedProperty currentMapping = m_MappingList.GetArrayElementAtIndex(index);
            SerializedProperty audioAssetProperty = currentMapping.FindPropertyRelative("m_AudioAsset");

            string guid;

            if (audioAssetProperty.objectReferenceValue == null)
            {
                OnSelected?.Invoke(null);
                return;
            }

            if (!AssetDatabase.TryGetGUIDAndLocalFileIdentifier(audioAssetProperty.objectReferenceValue, out guid, out long _))
            {
                throw new Exception("Something went terribly wrong. kill yourself");
            }

            string path = AssetDatabase.GUIDToAssetPath(guid);
            AudioAsset audioAsset = AssetDatabase.LoadAssetAtPath<AudioAsset>(path);

            OnSelected?.Invoke(audioAsset);
        }
    }
}
