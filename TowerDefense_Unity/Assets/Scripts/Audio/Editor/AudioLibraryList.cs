using UnityEditor;
using UnityEngine;

namespace Game.Audio.Editor
{
    public class AudioLibraryList
    {
        public event System.Action<AudioAssetLibrary> OnSelected;

        private SelectableList m_LibraryList;
        private Vector2 m_ScrollPosition = Vector2.zero;

        private string[] m_AssetGuids;
        private string[] m_AssetPaths;
        private string[] m_AssetLabels;

        public AudioLibraryList()
        {
            m_LibraryList = new SelectableList(DrawElement);
            m_LibraryList.OnSelect += OnSelect;
        }

        public void DoList()
        {
            FetchResources();
            GUILayout.Label($"{nameof(AudioLibraryList)}");
            m_ScrollPosition = EditorGUILayout.BeginScrollView(m_ScrollPosition);
            m_LibraryList.DoList(m_AssetLabels.Length, m_ScrollPosition);
            EditorGUILayout.EndScrollView();
        }

        private void OnSelect(int index)
        {
            OnSelected?.Invoke(AssetDatabase.LoadAssetAtPath<AudioAssetLibrary>(m_AssetPaths[index]));
        }

        private void DrawElement(int index)
        {
            GUILayout.Label(m_AssetLabels[index]);
        }

        private void FetchResources()
        {
            if (Event.current.type == EventType.Layout)
            {
                m_AssetGuids = AssetDatabase.FindAssets($"t:{nameof(AudioAssetLibrary).ToLower()}");
                m_AssetPaths = GUIDSToAssetPaths(m_AssetGuids);
                m_AssetLabels = PathsToLabels(m_AssetPaths);
            }
        }

        private string[] GUIDSToAssetPaths(string[] guids)
        {
            string[] paths = new string[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                paths[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
            }
            return paths;
        }

        private string[] PathsToLabels(string[] paths)
        {
            string[] labels = new string[paths.Length];

            for (int i = 0; i < paths.Length; i++)
            {
                labels[i] = paths[i].Replace("Assets/","").Replace(".asset", "");
            }

            return labels;
        }
    }
}