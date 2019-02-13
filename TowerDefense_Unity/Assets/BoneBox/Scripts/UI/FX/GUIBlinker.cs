using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BoneBox.UI.FX
{
	using Core;
	using Base;

	public class GUIBlinker : GUIColoringBase
	{
		[SerializeField] private bool m_IsEnabled = true;
		[SerializeField, Tooltip("How fast to blink in seconds.")]
        private float m_Speed = 1.0f;

		private float m_Timer;

		public bool IsEnabled
		{
			get
			{
				return m_IsEnabled;
			}
			set
			{
				m_IsEnabled = value;

				if (m_IsEnabled == false)
					ResetColor();
			}
		}

		public float Speed { get { return m_Speed; } set { m_Speed = value; } }
        public float Timer => m_Timer;

        protected virtual void Update()
		{
			if (m_IsEnabled)
			{
				m_Timer += Time.deltaTime;

				if (m_Timer >= m_Speed)
				{
					m_Timer = 0.0f;

					Color currentColor = Renderer.GetColor();
					MemoryUtility.Switch(ref currentColor, OriginalColor, TargetColor);
					Renderer.SetColor(currentColor);
				}
			}
		}
	}
}
