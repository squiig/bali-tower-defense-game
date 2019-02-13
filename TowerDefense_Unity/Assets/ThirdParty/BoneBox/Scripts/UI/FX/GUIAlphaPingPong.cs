using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BoneBox.UI.FX
{
    using Base;

    public class GUIAlphaPingPong : GUIColoringBase
    {
        [SerializeField] private float m_Speed = 1.0f;
        [SerializeField] private bool m_IgnoreTimeScale = true;

        private float m_Timer;

        public float Speed
        {
            get {
                return m_Speed;
            }
            set {
                m_Speed = value;
            }
        }

        public bool IgnoreTimeScale => m_IgnoreTimeScale;
        public float Timer => m_Timer;

		protected virtual void Update()
		{
			m_Timer += Time.unscaledDeltaTime * m_Speed;

			Renderer.SetAlpha(Mathf.PingPong(m_Timer, 1.0f));
		}
	}
}
