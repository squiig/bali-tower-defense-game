using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BoneBox.UI.CursorCustomization
{
	[CreateAssetMenu(menuName = BoneBox.ASSET_MENU_ROOT + "/" + ASSET_MENU_LABEL, fileName = "New " + ASSET_MENU_LABEL)]
	public class Cursor : ScriptableObject
	{
		public const string ASSET_MENU_LABEL = "Cursor";

		public static readonly Data DEFAULT = new Data(null, Vector2.zero);

		[SerializeField]
		private Data m_Data;

        public Texture2D Texture => m_Data.Texture;
        public Vector2 Hotspot => m_Data.Hotspot;

		[System.Serializable]
		public struct Data
		{
			[SerializeField, Tooltip("The texture to use for the cursor or null to set the default cursor. Note that a texture needs to be imported with \"Read/Write enabled\" in the texture importer (or using the \"Cursor\" defaults), in order to be used as a cursor.")]
			private Texture2D m_Texture;

			[SerializeField, Tooltip("The offset from the top left of the texture to use as the target point (must be within the bounds of the cursor).")]
			private Vector2 m_Hotspot;

			public Data(Texture2D texture, Vector2 hotspot)
			{
				m_Texture = texture;
				m_Hotspot = hotspot;
			}

			public Texture2D Texture {
				get {
					return m_Texture;
				}
			}

			public Vector2 Hotspot {
				get {
					return m_Hotspot;
				}
			}
		}
    }
}
