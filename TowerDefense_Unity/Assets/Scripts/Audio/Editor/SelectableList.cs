using System;
using UnityEngine;

namespace Game.Audio.Editor
{
	/// <summary>
	/// Draws a list with selectable elements
	/// </summary>
	public class SelectableList
	{
		public event Action<int> OnSelect;
		public event Action<int> OnDrawElement;

		private int _SelectedElementIndex = -1;
		private Rect[] _ElementRects;

		private readonly GUILayoutOption[] _Options =
		{
			GUILayout.ExpandWidth(true),
			GUILayout.ExpandHeight(false)
		};

		public int SelectedElementIndex => _SelectedElementIndex;

		public SelectableList(Action<int> drawElementCallback)
		{
			OnDrawElement += drawElementCallback;
		}

		public void ResetSelection()
		{
			SelectElement(-1);
		}

		public void DoList(int elementCount, Vector2 scrollOffset)
		{
			_ElementRects = new Rect[elementCount];
			for (int i = 0; i < elementCount; i++)
			{
				if (i == _SelectedElementIndex)
				{
					DrawSelectedElement(i);
				}
				else
				{
					DrawElement(i);
				}

				_ElementRects[i] = GUILayoutUtility.GetLastRect();
			}

			if (OnMouseButton(0))
			{
				CheckSelection(Event.current.mousePosition);
			}
		}

		private void DrawElement(int index)
		{
			GUILayout.BeginVertical(GUI.skin.box, _Options);
			OnDrawElement?.Invoke(index);
			GUILayout.EndVertical();
		}

		private void DrawSelectedElement(int index)
		{
			Color resetColor = GUI.color;
			GUI.color = Color.HSVToRGB(0.5f, 0.5f, 1);
			GUILayout.BeginVertical(GUI.skin.box, _Options);
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
			for (int i = 0; i < _ElementRects.Length; i++)
			{
				if (_ElementRects[i].Contains(mousePosition))
				{
					SelectElement(i);
					return;
				}
			}
		}

		private void SelectElement(int i)
		{
			OnSelect?.Invoke(i);
			_SelectedElementIndex = i;
		}
	}
}
