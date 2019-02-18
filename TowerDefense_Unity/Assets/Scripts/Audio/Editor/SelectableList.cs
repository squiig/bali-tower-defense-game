using System;
using UnityEngine;
using UnityEditor;


namespace Game.Audio.Editor
{
    /// <summary>
    /// Draws a list with selectable elements
    /// </summary>
    public class SelectableList
    {
        public event Action<int> OnSelect;
        public event Action<int> OnDrawElement;
        private int m_SelectedElement = -1;

        private readonly GUILayoutOption[] m_Options =
        {
            GUILayout.ExpandWidth(true),
            GUILayout.ExpandHeight(false)
        };

        private Rect[] m_ElementRects;

        public SelectableList(System.Action<int> drawElementCallback)
        {
            OnDrawElement += drawElementCallback;
        }

        public void ResetSelection()
        {
            SelectElement(-1);
        }

        public void DoList(int elementCount, Vector2 scrollOffset)
        {
            m_ElementRects = new Rect[elementCount];
            for (int i = 0; i < elementCount; i++)
            {
                if (i == m_SelectedElement)
                {
                    DrawSelectedElement(i);
                }
                else
                {
                    DrawElement(i);
                }

                m_ElementRects[i] = GUILayoutUtility.GetLastRect();
            }

            if (OnMouseButton(0))
            {
                CheckSelection(Event.current.mousePosition);
            }
        }

        private void DrawElement(int index)
        {
            GUILayout.BeginVertical(GUI.skin.box, m_Options);
            OnDrawElement?.Invoke(index);
            GUILayout.EndVertical();
        }

        private void DrawSelectedElement(int index)
        {
            Color resetColor = GUI.color;
            GUI.color = Color.cyan;
            GUILayout.BeginVertical(GUI.skin.box, m_Options);
            GUI.color = resetColor;

            OnDrawElement?.Invoke(index);
            GUILayout.EndVertical();
        }

        private bool OnMouseButton(int button)
        {
            return Event.current.isMouse
                   && Event.current.type == EventType.MouseDown
                   && Event.current.button == 0;

        }

        private void CheckSelection(Vector2 mousePosition)
        {
            for (int i = 0; i < m_ElementRects.Length; i++)
            {
                if (m_ElementRects[i].Contains(mousePosition))
                {
                    SelectElement(i);
                    return;
                }
            }
        }

        private void SelectElement(int i)
        {
            OnSelect?.Invoke(i);
            m_SelectedElement = i;
        }
    }
}