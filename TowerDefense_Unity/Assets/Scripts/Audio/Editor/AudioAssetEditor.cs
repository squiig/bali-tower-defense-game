using Game.Utils.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

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

		private SerializedProperty _AudioMixerGroup;
		private SerializedProperty _PitchMin;
		private SerializedProperty _PitchMax;
		private SerializedProperty _AvoidRepetition;
		private SerializedProperty _Volume;
		private SerializedProperty _Pan;

		private Vector3 _ScrollVector;

		private readonly GUIContent _AvoidRepetitionContent = new GUIContent("Avoid repetition", "Avoids repeating the same clip twice");
		private readonly GUIContent _VolumeLabel = new GUIContent("Volume", "Changes the loudness of this asset");
		private readonly GUIContent _PanLabel = new GUIContent("Pan (Left - Right)", "Which side should the 2D audio be played at");
		private readonly GUIContent _PitchLabel = new GUIContent("Pitch range", "Changes the pitch range the audio asset is randomly played at");
		private readonly GUIContent _MixerLabel = new GUIContent("Audio mixer group", "The channel in the mixer the audio is supposed to play on");

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
			_PitchMin = _SerializedTarget.FindProperty("_PitchMin");
			_PitchMax = _SerializedTarget.FindProperty("_PitchMax");
			_Pan = _SerializedTarget.FindProperty("_Pan");
			_AudioMixerGroup = _SerializedTarget.FindProperty("_AudioMixerGroup");
			_Volume = _SerializedTarget.FindProperty("_Volume");
			_AvoidRepetition = _SerializedTarget.FindProperty("_AvoidRepetition");
			_SelectableList.ResetSelection();
		}

		public void AddElement()
		{
			_ClipList.InsertArrayElementAtIndex(0);
		}

		public void AddClip(AudioClip clip)
		{
			int length = _ClipList.arraySize;
			_ClipList.InsertArrayElementAtIndex(length);
			_ClipList.GetArrayElementAtIndex(length)
				.objectReferenceValue = clip;
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

			GUILayout.Label($"{nameof(AudioAssetEditor)}");

			DrawProperties();
			DrawClipList();

			if (_SerializedTarget.hasModifiedProperties)
			{
				_SerializedTarget.ApplyModifiedProperties();
			}

			GUILayout.EndScrollView();
		}

		public void DrawProperties()
		{
			GUILayout.BeginVertical(GUI.skin.box);

			EditorGUILayout.LabelField(_VolumeLabel);
			_Volume.floatValue = EditorGUILayout.FloatField(_Volume.floatValue);
			EditorGUILayout.LabelField(_PanLabel);
			_Pan.floatValue = EditorGUILayout.Slider(_Pan.floatValue, -1f, 1f);

			EditorGUILayout.LabelField(_PitchLabel);
			EditorScriptUtil.RangeSlider(
				_PitchMin,
				_PitchMax,
				PITCH_MINIMUM,
				PITCH_MAXIMUM);

			EditorGUILayout.ObjectField(_AudioMixerGroup, typeof(AudioMixerGroup));
			_AvoidRepetition.boolValue = EditorGUILayout.Toggle(_AvoidRepetitionContent, _AvoidRepetition.boolValue);

			GUILayout.EndVertical();
		}

		public void DrawClipList()
		{
			GUILayout.BeginVertical(GUI.skin.box);
			GUILayout.BeginHorizontal();
			GUILayout.Label("Sound clips");
			
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


			_SelectableList.DoList(_ClipList.arraySize, _ScrollVector);
			GUILayout.EndVertical();

			AudioClip[] audioclips = EditorScriptUtil.FilteredDrop<AudioClip>();

			if (audioclips != null)
			{
				foreach (AudioClip clip in audioclips)
					AddClip(clip);
			}
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
