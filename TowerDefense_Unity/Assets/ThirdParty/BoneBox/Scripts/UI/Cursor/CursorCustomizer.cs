using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UCursor = UnityEngine.Cursor;

namespace BoneBox.UI.CursorCustomization
{
	using Core;

	public class CursorCustomizer : DDOLSingleton<CursorCustomizer>
	{
		[SerializeField] protected CursorState m_StartingState = null;

		protected CursorState.Data m_CurrentState = CursorState.DEFAULT;
		protected CursorState.Data m_FocusedState = CursorState.DEFAULT;
        
        public CursorState StartingState => m_StartingState;
        public CursorState.Data CurrentState => m_CurrentState;

        private void SetCursor(Texture2D texture, Vector2 hotspot, CursorMode renderMode)
		{
			UCursor.SetCursor(texture, hotspot, renderMode);
			m_CurrentState.RenderMode = renderMode;
		}

		public void SetCursorRaw(Cursor.Data cursorData, CursorMode renderMode)
		{
			SetCursor(cursorData.Texture, cursorData.Hotspot, renderMode);
			m_CurrentState.RawCursor = cursorData;
		}

		public void SetCursor(Cursor cursor, CursorMode renderMode)
		{
			SetCursor(cursor.Texture, cursor.Hotspot, renderMode);
			m_CurrentState.Cursor = cursor;
		}

		public void SetLockMode(CursorLockMode lockMode)
		{
			UCursor.lockState = lockMode;
			m_CurrentState.LockMode = lockMode;
		}

		public void SetVisibility(bool isVisible)
		{
			UCursor.visible = isVisible;
			m_CurrentState.IsVisible = isVisible;
		}

		public void ApplyRawState(CursorState.Data data)
		{
			if (data == null)
			{
				Debug.LogError("State data can not be null!", gameObject);
				return;
			}

			SetCursorRaw(data.RawCursor, data.RenderMode);
			SetLockMode(data.LockMode);
			SetVisibility(data.IsVisible);
		}

		public void ResetState()
		{
			ApplyRawState(CursorState.DEFAULT);
		}

		private void OnApplicationFocus(bool focus)
		{
			if (focus)
			{
				if (m_FocusedState != null)
				{
					ApplyRawState(m_FocusedState);
				}
			}
			else
			{
				m_FocusedState = m_CurrentState ?? m_StartingState.CopyData();

				ResetState();
			}
		}

		private void OnApplicationQuit()
		{
			ResetState();
		}

		public void Apply(Cursor cursor, bool isVisible = true, CursorLockMode lockMode = CursorLockMode.None, CursorMode renderMode = CursorMode.Auto)
		{
			SetCursor(cursor, renderMode);
			SetLockMode(lockMode);
			SetVisibility(isVisible);
		}

		public void ApplyState(CursorState state)
		{
			if (state == null)
			{
				Debug.LogError("State can not be null!", gameObject);
				return;
			}

			Apply(state.Cursor, state.IsVisible, state.LockMode, state.RenderMode);
		}

		private void Start()
		{
			ApplyState(m_StartingState);

			m_FocusedState = m_CurrentState;
		}
	}
}
