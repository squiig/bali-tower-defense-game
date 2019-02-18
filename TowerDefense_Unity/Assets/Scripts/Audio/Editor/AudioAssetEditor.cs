using UnityEditor;
using UnityEngine;

namespace Game.Audio.Editor
{
    public class AudioAssetEditor
    {
        private SelectableList m_SelectableList;

        private AudioAsset m_RawTarget;
        private SerializedObject m_SerializedTarget;
        private SerializedProperty m_ClipList;

        private Vector3 m_ScrollVector;

        public AudioAssetEditor()
        {
            m_SelectableList = new SelectableList(DrawElement);
        }

        private void DrawElement(int index)
        {
            SerializedProperty clip = m_ClipList.GetArrayElementAtIndex(index);
            EditorGUILayout.ObjectField(clip);
        }

        public void SetTarget(AudioAsset audioAsset)
        {
            if (audioAsset == null)
            {
                m_RawTarget = null;
                m_SerializedTarget = null;
                m_ClipList = null;
                m_SelectableList.ResetSelection();
                return;
            }

            m_RawTarget = audioAsset;
            m_SerializedTarget = new SerializedObject(m_RawTarget);
            m_ClipList = m_SerializedTarget.FindProperty("m_AudioClips");
        }

        public void DoAssetEditor()
        {
            if (m_RawTarget == null)
            {
                return;
            }
            m_ScrollVector = GUILayout.BeginScrollView(m_ScrollVector);
            GUILayout.Label($"{nameof(AudioAssetEditor)}");
            m_SelectableList.DoList(m_ClipList.arraySize, m_ScrollVector);
            m_SerializedTarget.ApplyModifiedProperties();
            GUILayout.EndScrollView();
        }
    }
}