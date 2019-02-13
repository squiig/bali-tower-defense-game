using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BoneBox.UI.CursorCustomization
{
	[CreateAssetMenu(menuName = BoneBox.ASSET_MENU_ROOT + "/" + ASSET_MENU_LABEL, fileName = "New " + ASSET_MENU_LABEL)]
	public class CursorState : ScriptableObject
	{
		public const string ASSET_MENU_LABEL = "CursorState";

		public static readonly Data DEFAULT = new Data(Cursor.DEFAULT, true, CursorMode.Auto, CursorLockMode.None);

		[SerializeField]
		private Data m_Data = null;

		public Data CopyData()
		{
			return new Data(m_Data);
		}

        public bool IsVisible => m_Data.IsVisible;
        public Cursor Cursor => m_Data.Cursor;
        public CursorMode RenderMode => m_Data.RenderMode;
        public CursorLockMode LockMode => m_Data.LockMode;

        [System.Serializable]
		public class Data
		{
			[SerializeField] private Cursor m_Cursor = null;
			[SerializeField] private bool m_IsVisible = true;

			[SerializeField, Tooltip("Allow this cursor to render as a hardware cursor on supported platforms, or force software cursor.")]
			private CursorMode m_RenderMode = CursorMode.Auto;

			[SerializeField, Tooltip("Determines whether the hardware pointer is locked to the center of the view, constrained to the window, or not constrained at all.")]
			private CursorLockMode m_LockMode = CursorLockMode.None;

			private Cursor.Data m_RawCursor;

			public Data(Cursor.Data rawCursor, bool isVisible, CursorMode renderMode, CursorLockMode lockMode)
			{
				m_RawCursor = rawCursor;
				m_IsVisible = isVisible;
				m_RenderMode = renderMode;
				m_LockMode = lockMode;
			}

			public Data(Cursor cursor, bool isVisible, CursorMode renderMode, CursorLockMode lockMode)
			{
				m_Cursor = cursor;
				m_IsVisible = isVisible;
				m_RenderMode = renderMode;
				m_LockMode = lockMode;

				UpdateRawCursor();
			}

			public Data(Data data)
			{
				m_Cursor = data.m_Cursor;
				m_IsVisible = data.m_IsVisible;
				m_RenderMode = data.m_RenderMode;
				m_LockMode = data.m_LockMode;

				UpdateRawCursor();
			}

			private void UpdateRawCursor()
			{
				m_RawCursor = new Cursor.Data(m_Cursor.Texture, m_Cursor.Hotspot);
			}

			public Cursor.Data RawCursor {
				get {
					return m_RawCursor;
				}
				set {
					m_RawCursor = value;
				}
			}

			public Cursor Cursor {
				get {
					return m_Cursor;
				}
				set {
					m_Cursor = value;
					UpdateRawCursor();
				}
			}

			public bool IsVisible {
				get {
					return m_IsVisible;
				}
				set {
					m_IsVisible = value;
				}
			}

			public CursorMode RenderMode {
				get {
					return m_RenderMode;
				}
				set {
					m_RenderMode = value;
				}
			}

			public CursorLockMode LockMode {
				get {
					return m_LockMode;
				}
				set {
					m_LockMode = value;
				}
			}
		}
	}
}
