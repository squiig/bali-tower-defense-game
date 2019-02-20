using System.IO;
using UnityEditor;
using UnityEngine;

namespace Game.Audio.Editor
{
    public class CreateAudioAssetPopup : EditorWindow
    {
        public System.Action<AudioAsset> OnCreated;

        /// <summary>
        /// File location the asset will be stored in.
        /// </summary>
        private const string FILEPATH = "Assets/Audio/AudioAssets/";
        private const string EXTENSION = ".asset";
        private const string ERR_FILE_EXISTS = "<color=red>Error: File already exists</color>";

        private string _AssetName = "name";
        private GUIStyle _RichTextStyle;
        private bool _ShowFileExistsMessage;
        private bool _SelectField = true;

        private string FinalPath => FILEPATH + _AssetName + EXTENSION;

        public void OnLostFocus()
        {
            Close();
        }

        public void Awake()
        {
            titleContent = new GUIContent("Create AudioAsset");
            _RichTextStyle = new GUIStyle
            {
                richText = true,
                alignment = TextAnchor.MiddleCenter
            };

            ShowUtility();


            Vector2 mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            Vector2 size = new Vector2(200, 110);
            position = new Rect(mousePos - size / 2, size);
        }

        private void CreateAndClose()
        {
            OnCreated?.Invoke(AssetUtil.Create<AudioAsset>(FinalPath));
            Close();
        }

        private bool KeyPressed(KeyCode keyCode)
        {
            return Event.current.isKey && Event.current.keyCode == keyCode;
        }

        private bool IsLayoutOrRepaint()
        {
            return Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout;
        }

        public void OnGUI()
        {
            GUILayout.Label("Create Audio Asset:");

            GUILayout.Space(25);
            GUILayout.Label("Name:");

            GUI.SetNextControlName("namefield");
            _AssetName = EditorGUILayout.TextField(_AssetName, GUILayout.ExpandWidth(true));

            if (GUILayout.Button("Create Asset") || KeyPressed(KeyCode.Return))
            {
                _ShowFileExistsMessage = File.Exists(FinalPath);
                if (!_ShowFileExistsMessage)
                {
                    CreateAndClose();
                }
            }

            if (_ShowFileExistsMessage && IsLayoutOrRepaint())
            {
                EditorGUILayout.LabelField(ERR_FILE_EXISTS, _RichTextStyle);
            }

            if (_SelectField)
            {
                EditorGUI.FocusTextInControl("namefield");
                _SelectField = false;
            }
        }
    }
}