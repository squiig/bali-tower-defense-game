using Game.Utils.Editor;

using UnityEditor;
using UnityEngine;

namespace Game.Audio.Editor
{
    /// <summary>
    /// Used to render and control a selectable list of all audio libraries in the project
    /// /// </summary>
    public class AudioLibraryList
    {
        public event System.Action<AudioAssetLibrary> OnSelected;
        public event System.Action OnRequestRepaint;

        private readonly SelectableList _SelectionList;
        private Vector2 _ScrollPosition = Vector2.zero;

        private string[] _AssetGuids;
        private string[] _AssetPaths;
        private string[] _AssetLabels;

        public AudioLibraryList()
        {
            _SelectionList = new SelectableList(DrawElement);
            _SelectionList.OnSelect += OnSelect;
        }

        public void DoList()
        {
            FetchResources();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"{nameof(AudioLibraryList)}");

            if (GUILayout.Button("Create New Library"))
            {
                EditorWindow.CreateInstance<CreateAudioLibraryAssetPopup>()
                    .SetFolder("Assets/Audio/Resources/")
                    .OnCreated += OnNewElementcreated;
            }

            if (GUILayout.Button("Delete"))
            {
                RemoveSelectedElement();
            }
            GUILayout.EndHorizontal();
            _ScrollPosition = EditorGUILayout.BeginScrollView(_ScrollPosition);
            _SelectionList.DoList(_AssetLabels.Length, _ScrollPosition);
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

        private void OnNewElementcreated(AudioAssetLibrary element)
        {
            OnRequestRepaint?.Invoke();
        }

        private void RemoveSelectedElement()
        {
            int index = _SelectionList.SelectedElementIndex;

            if (index == -1) // nothing selected delete
                return;

            string name = _AssetLabels[index];
            EditorWindow.CreateInstance<ConfirmActionPopup>()
                .SetQuestion($"Are you sure you want to delete {name}?")
                .OnConfirm += () => AssetDatabase.DeleteAsset(_AssetPaths[index]);
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