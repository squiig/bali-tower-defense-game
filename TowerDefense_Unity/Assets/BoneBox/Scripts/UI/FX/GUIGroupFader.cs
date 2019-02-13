using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BoneBox.UI.FX
{
	using Base;

	/// <summary>
	/// Transitions a CanvasGroup from the original color to the target color.
	/// </summary>
	public class GUIGroupFader : GUIGroupFXBase
	{
		private Action m_Finished;

		public event Action Finished {
			add
			{
				m_Finished += value;
			}
			remove
			{
				m_Finished -= value;
			}
		}

		[SerializeField] private float m_TargetAlpha;
		[SerializeField] private bool m_AutoStart;
		[SerializeField] private bool m_IsReversed;
		[SerializeField] private bool m_IgnoreTimeScale = true;
		[SerializeField] private bool m_AlsoToggleInteractability = true;
		[SerializeField] private bool m_AlsoToggleRaycastBlocking = true;
		[SerializeField] private float m_Speed = 1.0f;
		[SerializeField] private AnimationCurve m_Curve;

		private Coroutine m_CurrentFadeRoutine;

        private float m_OriginalAlpha;
        
		public float OriginalAlpha { get { return m_OriginalAlpha; } }
		public float TargetAlpha { get { return m_TargetAlpha; } set { m_TargetAlpha = value; } }

		public bool IsReversed { get { return m_IsReversed; } set { m_IsReversed = value; } }
		public bool IgnoreTimeScale { get { return m_IgnoreTimeScale; } set { m_IgnoreTimeScale = value; } }
		public float Speed { get { return m_Speed; } set { m_Speed = value; } }

		protected override void Awake()
		{
			base.Awake();

            m_OriginalAlpha = CanvasGroup.alpha;
		}

		protected virtual void OnEnable()
		{
			if (m_AutoStart)
			{
				Fade();
			}
		}

        protected virtual void OnFadeFinished()
        {
			m_Finished?.Invoke();
        }

		/// <summary>
		/// Sets the group alpha to the original alpha.
		/// </summary>
		public void ResetAlpha()
		{
			CanvasGroup.alpha = m_OriginalAlpha;
		}

		private IEnumerator FadeRoutine(float from, float to, float speed, bool ignoreTimeScale)
		{
			float timer = 0.0f;

			while (!Mathf.Approximately(CanvasGroup.alpha, to))
			{
				timer += (ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime) * speed;
				CanvasGroup.alpha = Mathf.Lerp(from, to, m_Curve.Evaluate(timer));
				yield return null;
			}

			// For mathematical good measure
			CanvasGroup.alpha = to;
			
			if (m_AlsoToggleInteractability)
			{
				CanvasGroup.interactable = IsFullyVisible;
			}

			if (m_AlsoToggleRaycastBlocking)
			{
				CanvasGroup.blocksRaycasts = IsFullyVisible;
			}

            OnFadeFinished();

			m_CurrentFadeRoutine = null;
		}

		public void Fade(float from, float to, float speed, bool reversed = false, bool forced = true, bool ignoreTimeScale = true)
		{
            if (m_CurrentFadeRoutine != null && !forced)
                return;

			if (reversed)
			{
				CanvasGroup.alpha = to;
				m_CurrentFadeRoutine = StartCoroutine(FadeRoutine(to, from, speed, ignoreTimeScale));
			}
			else
			{
				m_CurrentFadeRoutine = StartCoroutine(FadeRoutine(from, to, speed, ignoreTimeScale));
			}
		}

		public void Fade(float from, float to) 
		{
			Fade(from, to, m_Speed, m_IsReversed, m_IgnoreTimeScale);
		}

		public void Fade(float to, bool reversed)
		{
			Fade(CanvasGroup.alpha, to, m_Speed, reversed, m_IgnoreTimeScale);
		}

		public void Fade(float to)
		{
			Fade(CanvasGroup.alpha, to, m_Speed, m_IsReversed, m_IgnoreTimeScale);
		}

		public void Fade(bool reversed)
		{
			Fade(CanvasGroup.alpha, m_TargetAlpha, m_Speed, reversed, m_IgnoreTimeScale);
		}

		public void Fade()
		{
			Fade(CanvasGroup.alpha, m_TargetAlpha, m_Speed, m_IsReversed, m_IgnoreTimeScale);
		}
	}
}
