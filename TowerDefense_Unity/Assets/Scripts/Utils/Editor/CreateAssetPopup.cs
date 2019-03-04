using System.IO;

using UnityEditor;
using UnityEngine;

namespace Game.Utils.Editor
{
    public abstract class CreateAssetPopup<T> : EditorWindow where T : ScriptableObject, new()
    {
        public System.Action<T> OnCreated;

        /// <summary>
        /// File location the asset will be stored in.
        /// </summary>
        private string _FolderPath = "Assets/";
        private const string ERR_FILE_EXISTS = "<color=red>Error: File already exists</color>";

        private string _AssetName = "name";
        private GUIStyle _RichTextStyle;
        private bool _ShowFileExistsMessage;
        private bool _SelectField = true;


        public void OnLostFocus()
        {
            Close();
        }

        public void Awake()
        {
            titleContent = new GUIContent($"Create {typeof(T).Name}");
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

        /// <summary>
        /// Folder leading up to the target folder
        /// </summary>
        /// <param name="folder"></param>
        /// <returns>instance for function chaining</returns>
        public CreateAssetPopup<T> SetFolder(string folder)
        {
            _FolderPath = folder;

            return this;
        }

        private void CreateAndClose()
        {
            OnCreated?.Invoke(AssetUtil.Create<T>(_FolderPath, _AssetName));
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
            GUILayout.Label($"Create {typeof(T).Name}:");

            GUILayout.Space(25);
            GUILayout.Label("Name:");

            GUI.SetNextControlName("namefield");
            _AssetName = EditorGUILayout.TextField(_AssetName, GUILayout.ExpandWidth(true));

            if (GUILayout.Button("Create Asset") || KeyPressed(KeyCode.Return))
            {
                _ShowFileExistsMessage = AssetUtil.Exists(_FolderPath, _AssetName);
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