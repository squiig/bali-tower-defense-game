using System.Collections;
using System.Collections.Generic;

namespace BoneBox.UI
{
	/*
	 * WIP
	 */

	/// <summary>
	/// Use this class to manage the layering of hideable and/or navigable UI.
	/// </summary>
	public class UIStack
	{
		private Stack<IHideableUI> m_Stack;

        public bool IsEmpty => m_Stack.Count == 0;
        public bool IsCurrentLayerNavigable => (m_Stack.Peek() as INavigableUI) != null;

        public UIStack()
		{
			m_Stack = new Stack<IHideableUI>();
		}

		public UIStack(int capacity)
		{
			m_Stack = new Stack<IHideableUI>(capacity);
		}

		public UIStack(IEnumerable<IHideableUI> collection)
		{
			m_Stack = new Stack<IHideableUI>(collection);
		}

		/// <summary>
		/// Clears the stack and hides all UI that were shown.
		/// </summary>
		public void Stop()
		{
			foreach (IHideableUI ui in m_Stack)
			{
				ui.Hide();
			}

			m_Stack.Clear();
		}

		/// <summary>
		/// Stops the current stack and starts over by pushing and showing the new origin UI.
		/// </summary>
		/// <param name="origin"></param>
		public void StartNew(IHideableUI origin)
		{
			Stop();
			OpenNewLayer(origin);
		}

		/// <summary>
		/// Hides the current layer WITHOUT removing it from the UI stack.
		/// </summary>
		public void HideCurrentLayer()
		{
			if (IsEmpty)
				return;

			m_Stack.Peek().Hide();
		}

		public void ShowCurrentLayer()
		{
			if (IsEmpty)
				return;

			m_Stack.Peek().Show();
		}

		/// <summary>
		/// Hides the current layer AND removes it from the UI stack.
		/// </summary>
		public void CloseCurrentLayer()
		{
			if (IsEmpty)
				return;

			m_Stack.Pop().Hide();

			// Focus UI navigation on the next layer if there is one
			TryFocusUINavigation();
		}

		/// <summary>
		/// Adds and shows a new layer.
		/// </summary>
		/// <param name="ui"></param>
		public void OpenNewLayer(IHideableUI ui)
		{
			m_Stack.Push(ui);
			ui.Show();
		}

		public bool TryFocusUINavigation()
		{
			if (IsEmpty)
				return false;

			IHideableUI currentLayer = m_Stack.Peek();
			if (currentLayer == null || !currentLayer.IsVisible || !IsCurrentLayerNavigable)
				return false;

			(currentLayer as INavigableUI).SelectUINavigationOrigin();
			return true;
		}
	}
}
