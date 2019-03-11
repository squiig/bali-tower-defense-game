using Game.Utils.Editor;

using UnityEditor;
using UnityEngine;

using System;

namespace Game.Audio.Editor
{
    /// <summary>
    /// Used to render and control a window to edit an audio library asset
    /// </summary>
    public class AudioLibraryEditor
    {
        private readonly SelectableList _SelectableAudioAssetList;
        public event Action<AudioAsset> OnSelected;
        public event Action<AudioAsset> OnPreview;
        public event Action OnRequestRepaint;

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
            _MappingList = _SerializedTarget.FindProperty("_AudioAssetIdentifierMappings");
        }

        public void DoLibraryEditor()
        {
            if (_RawTarget == null)
            {
                return;
            }

            _ScrollVector = GUILayout.BeginScrollView(_ScrollVector);

            GUILayout.Label($"{nameof(AudioLibraryEditor)}");
            DrawAssetMappingList();

			_SerializedTarget.ApplyModifiedProperties();
            GUILayout.EndScrollView();
        }

        private void DrawAssetMappingList()	
        {
	        GUILayout.BeginVertical(GUI.skin.box);
	        GUILayout.BeginHorizontal();

	        GUILayout.Label($"Audio Asset Mappings");


			if (GUILayout.Button("Nuke"))
	        {
		        ScriptableObject.CreateInstance<ConfirmActionPopup>()
			        .SetQuestion("You're about to nuke all elements from this library, are you sure?").
			        OnConfirm += DeleteAllElements;
	        }

	        if (GUILayout.Button("Remove"))
	        {
		        RemoveSelectedElement();
	        }

	        if (GUILayout.Button("Add"))
	        {
		        EditorWindow.CreateInstance<CreateAudioAssetPopup>()
			        .SetFolder("Assets/Audio/AudioAssets/")
			        .OnCreated += AddElement;
	        }
	        GUILayout.EndHorizontal();
	        _SelectableAudioAssetList.DoList(_MappingList.arraySize, _ScrollVector);
	        GUILayout.EndVertical();
        }


        private void DrawElement(int index)
        {
            SerializedProperty currentMapping = _MappingList.GetArrayElementAtIndex(index);
            SerializedProperty audioAssetProperty = currentMapping.FindPropertyRelative("_AudioAsset");
            SerializedProperty audioIdentifierProperty = currentMapping.FindPropertyRelative("_Identifier");

            audioIdentifierProperty.stringValue = EditorGUILayout.TextField(audioIdentifierProperty.stringValue);

			GUILayout.BeginHorizontal();
            EditorGUILayout.ObjectField(audioAssetProperty);

            if (GUILayout.Button("Preview"))
            {
	            OnPreview?.Invoke((AudioAsset) audioAssetProperty.objectReferenceValue);
            }

            EditorGUILayout.EndHorizontal();
        }

        private void AddElement(AudioAsset asset)
        {
            int index = _MappingList.arraySize;
            _MappingList.InsertArrayElementAtIndex(_MappingList.arraySize);
            SerializedProperty currentMapping = _MappingList.GetArrayElementAtIndex(index);
            SerializedProperty audioAssetProperty = currentMapping.FindPropertyRelative("_AudioAsset");
            audioAssetProperty.objectReferenceValue = asset;
            _SerializedTarget.ApplyModifiedProperties();
            OnRequestRepaint?.Invoke();
        }

        private void RemoveSelectedElement()
        {
            int index = _SelectableAudioAssetList.SelectedElementIndex;

            if (index == -1)
            {
                return;
            }

            _MappingList.DeleteArrayElementAtIndex(index);
        }

        private void DeleteAllElements()
        {
            _MappingList.ClearArray();
            _SerializedTarget.ApplyModifiedProperties();
        }

        private void OnAudioAssetSelected(int index)
        {
            if (index == -1)
            {
                OnSelected?.Invoke(null);
                return;
            }

            SerializedProperty audioAssetProperty =
                EditorScriptUtil.FromPropertyRelativeFromIndex(_MappingList, index, "_AudioAsset");

            if (!EditorScriptUtil.TryFetchPropertyGuid(audioAssetProperty, out string guid))
            {
                OnSelected?.Invoke(null);
                return;
            }

            string path = AssetDatabase.GUIDToAssetPath(guid);
            AudioAsset audioAsset = AssetDatabase.LoadAssetAtPath<AudioAsset>(path);

            OnSelected?.Invoke(audioAsset);
		}
    }
}
