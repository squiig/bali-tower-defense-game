using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BoneBox.UI.FX
{
	/// <summary>
	/// WIP
	/// </summary>
	public class GUIGroupFadeSequence : MonoBehaviour
	{
		private Action m_Finished;

		public event Action Finished {
			add {
				m_Finished += value;
			}
			remove {
				m_Finished -= value;
			}
		}

		[SerializeField] private List<GUIGroupFader> m_Faders = new List<GUIGroupFader>();
		[SerializeField] private bool m_AutoStart;

		private int m_Counter = 0;

		void OnEnable()
		{
			if (m_AutoStart)
			{
				Next();
			}
		}

        protected virtual void OnFadeFinished()
        {
			m_Finished?.Invoke();
        }

		private void OnFaderFinished()
		{
			m_Faders[m_Counter].Finished -= OnFaderFinished;
			Next();
		}

		private void Next()
		{
			++m_Counter;

			if (m_Counter == m_Faders.Count)
			{
                OnFadeFinished();
				return;
			}

			m_Faders[m_Counter].Finished += OnFaderFinished;
			m_Faders[m_Counter].Fade();
		}
	}
}
