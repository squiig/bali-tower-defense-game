using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Game.Audio.Editor
{
    /// <summary>
    /// Displays a custom question confirmation popup to give the user a moment to reconsider what he's doing. Default: "Are you sure?"
    /// </summary>
    public class ConfirmActionPopup : EditorWindow
    {
        private string _QuestionText = "Are you sure?";

        public event System.Action OnConfirm;
        public event System.Action<bool> OnButton;

        public void Awake()
        {
            ShowUtility();
            Focus();

            titleContent = new GUIContent("Please Confirm");
            Vector2 mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            Vector2 size = new Vector2(320, 100);
            position = new Rect(mousePos - size / 2, size);
        }

        public ConfirmActionPopup SetQuestion(string question)
        {
            _QuestionText = question;
            return this;
        }

        /// <summary>
        /// In 0-3 words what the user is doing.
        /// </summary>
        /// <param name="context"></param>
        public void SetActionContext(string context)
        {
            titleContent = new GUIContent(context);
        }

        public void OnLostFocus()
        {
            Close();
        }

        public void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Label(_QuestionText, GUILayout.ExpandWidth(true));
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Yes"))
            {
                OnConfirm?.Invoke();
                OnButton?.Invoke(true);
                Close();
            }

            if (GUILayout.Button("No"))
            {
                OnButton?.Invoke(false);
                Close();
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
    }
}