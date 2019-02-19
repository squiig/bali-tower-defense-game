using UnityEditor;
using UnityEngine;

namespace Game.Audio.Editor
{
    /// <summary>
    /// Defines and manages editors for the audio system.
    /// </summary>
    public class AudioSystemWindow : EditorWindow
    {
        private float ViewWidth  => position.width / 3;
        private float ViewHeight => position.height;

        private Rect LibraryListView      => new Rect(0,             0, ViewWidth, ViewHeight);
        private Rect LibraryEditorView    => new Rect(ViewWidth,     0, ViewWidth, ViewHeight);
        private Rect AudioAssetEditorView => new Rect(ViewWidth * 2, 0, ViewWidth, ViewHeight);

        private readonly AudioLibraryList   _AudioLibraryList   = new AudioLibraryList();
        private readonly AudioLibraryEditor _AudioLibraryEditor = new AudioLibraryEditor();
        private readonly AudioAssetEditor   _AudioAssetEditor   = new AudioAssetEditor();

        [MenuItem("Window/AudioSystem")]
        public static void ShowWindow()
        {
            GetWindow<AudioSystemWindow>();
        }

        public void OnEnable()
        {
            titleContent = new GUIContent("Audio Editor Window");
            _AudioLibraryList.OnSelected += _AudioLibraryEditor.SetTarget;
            _AudioLibraryEditor.OnSelected += _AudioAssetEditor.SetTarget;
        }

        public void OnDisable()
        {
            _AudioLibraryList.OnSelected -= _AudioLibraryEditor.SetTarget;
            _AudioLibraryEditor.OnSelected -= _AudioAssetEditor.SetTarget;

        }

        public void Area(Rect view, Color color)
        {
            Color reset = GUI.color;
            GUI.color = color;
            GUILayout.BeginArea(view, GUI.skin.box);
            GUI.color = reset;
        }

        public void OnGUI()
        {
            Color color = new Color(0.8f,0.8f,0.8f);

            Area(AudioAssetEditorView, color);
            _AudioAssetEditor.DoAssetEditor();
            GUILayout.EndArea();

            Area(LibraryEditorView, color);
            _AudioLibraryEditor.DoLibraryEditor();
            GUILayout.EndArea();

            Area(LibraryListView, color);
            _AudioLibraryList.DoList();
            GUILayout.EndArea();

            if (Event.current.isMouse && Event.current.type == EventType.MouseDown)
            {
                Repaint();
            }
        }
    }
}