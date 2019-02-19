using UnityEditor;
using UnityEngine;

namespace Game.Audio.Editor
{
    public class AudioLibraryList
    {
        public event System.Action<AudioAssetLibrary> OnSelected;

        private readonly SelectableList _LibraryList;
        private Vector2 _ScrollPosition = Vector2.zero;

        private string[] _AssetGuids;
        private string[] _AssetPaths;
        private string[] _AssetLabels;

        public AudioLibraryList()
        {
            _LibraryList = new SelectableList(DrawElement);
            _LibraryList.OnSelect += OnSelect;
        }

        public void DoList()
        {
            FetchResources();
            GUILayout.Label($"{nameof(AudioLibraryList)}");
            _ScrollPosition = EditorGUILayout.BeginScrollView(_ScrollPosition);
            _LibraryList.DoList(_AssetLabels.Length, _ScrollPosition);
            EditorGUILayout.EndScrollView();
        }

        private void OnSelect(int index)
        {
            OnSelected?.Invoke(AssetDatabase.LoadAssetAtPath<AudioAssetLibrary>(_AssetPaths[index]));
        }

        private void DrawElement(int index)
        {
            GUILayout.Label(_AssetLabels[index]);
        }

        private void FetchResources()
        {
            if (Event.current.type == EventType.Layout)
            {
                _AssetGuids = AssetDatabase.FindAssets($"t:{nameof(AudioAssetLibrary).ToLower()}");
                _AssetPaths = GUIDSToAssetPaths(_AssetGuids);
                _AssetLabels = PathsToLabels(_AssetPaths);
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