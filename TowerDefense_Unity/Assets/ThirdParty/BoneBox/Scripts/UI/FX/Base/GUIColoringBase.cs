using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BoneBox.UI.FX.Base
{
	public class GUIColoringBase : GUIFXBase
	{
		private Color m_OriginalColor;

		[SerializeField] private Color m_TargetColor;

        public Color OriginalColor => m_OriginalColor;

		public Color TargetColor
        {
            get {
                return m_TargetColor;
            }
            set {
                m_TargetColor = value;
            }
        }

        protected override void Awake()
		{
			base.Awake();
			
			m_OriginalColor = Renderer.GetColor();
		}

		/// <summary>
		/// Sets the renderer color to the original color.
		/// </summary>
		public virtual void ResetColor()
		{
			Renderer.SetColor(m_OriginalColor);
		}
	}
}
