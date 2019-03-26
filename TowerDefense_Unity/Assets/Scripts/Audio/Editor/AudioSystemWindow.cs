using UnityEditor;
using UnityEngine;

namespace Game.Audio.Editor
{
	/// <summary>
	/// Defines and manages editors for the audio system.
	/// </summary>
	public class AudioSystemWindow : EditorWindow
	{
		private const int VIEW_COUNT = 3;
		private const float AREA_BG_LIGHTNESS = 0.8f;

		private float ViewWidth => position.width / VIEW_COUNT;
		private float ViewHeight => position.height;

		private Rect LibraryListView => new Rect(0, 0, ViewWidth, ViewHeight);
		private Rect LibraryEditorView => new Rect(ViewWidth, 0, ViewWidth, ViewHeight);
		private Rect AudioAssetEditorView => new Rect(ViewWidth * 2, 0, ViewWidth, ViewHeight);

		private readonly AudioLibraryList _AudioLibraryList = new AudioLibraryList();
		private readonly AudioLibraryEditor _AudioLibraryEditor = new AudioLibraryEditor();
		private readonly AudioAssetEditor _AudioAssetEditor = new AudioAssetEditor();

		private readonly AudioPreviewer _AudioPreviewer = new AudioPreviewer();

		private readonly Color _ViewBackgroundColor = new Color(AREA_BG_LIGHTNESS, AREA_BG_LIGHTNESS, AREA_BG_LIGHTNESS);

		private void Awake()
		{
			titleContent = new GUIContent("Audio System", "Used for editing what sounds belong to what identifier");
		}

		private void OnEnable()
		{
			_AudioPreviewer.Create();
			_AudioLibraryList.OnSelected += _AudioLibraryEditor.SetTarget;
			_AudioLibraryEditor.OnSelected += _AudioAssetEditor.SetTarget;
			_AudioLibraryEditor.OnPreview += _AudioPreviewer.Play;
			_AudioLibraryEditor.OnRequestRepaint += Repaint;
			_AudioLibraryList.OnRequestRepaint += Repaint;
		}

		private void OnDisable()
		{
			_AudioPreviewer.Remove();
			_AudioLibraryList.OnSelected -= _AudioLibraryEditor.SetTarget;
			_AudioLibraryEditor.OnSelected -= _AudioAssetEditor.SetTarget;
			_AudioLibraryEditor.OnPreview -= _AudioPreviewer.Play;
			_AudioLibraryEditor.OnRequestRepaint -= Repaint;
			_AudioLibraryList.OnRequestRepaint -= Repaint;
		}

		private void DrawInViewArea(Rect viewRect, System.Action drawFunc)
		{
			Color reset = GUI.color;
			GUI.color = _ViewBackgroundColor;
			GUILayout.BeginArea(viewRect, GUI.skin.box);
			GUI.color = reset;
			drawFunc.Invoke();
			GUILayout.EndArea();
		}

		private void OnGUI()
		{
			DrawInViewArea(AudioAssetEditorView, _AudioAssetEditor.DoAssetEditor);
			DrawInViewArea(LibraryEditorView, _AudioLibraryEditor.DoLibraryEditor);
			DrawInViewArea(LibraryListView, _AudioLibraryList.DoList);

			if (Event.current.isMouse && Event.current.type == EventType.MouseDown)
			{
				// This makes the editor window more responsive in this use case
				// since it uses custom selectable elements
				Repaint();
			}
		}
	}
}
