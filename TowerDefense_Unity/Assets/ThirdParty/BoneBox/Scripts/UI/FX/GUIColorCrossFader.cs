using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BoneBox.UI.FX
{
	using Base;

	/// <summary>
	/// Transition from the original color to the target color.
	/// </summary>
	public class GUIColorCrossFader : GUIColoringBase
	{
		private Action m_Finished;

		public event Action Finished
		{
			add
			{
				m_Finished += value;
			}
			remove
			{
				m_Finished -= value;
			}
		}

		[SerializeField] protected bool m_AutoStart;
		[SerializeField] protected bool m_IsReversed;
		[SerializeField] protected bool m_IgnoreTimeScale = true;
		[SerializeField] protected float m_Speed = 1.0f;
		[SerializeField] protected AnimationCurve m_Curve;

		private Coroutine m_CurrentCrossFadeRoutine;

		public bool IsReversed { get { return m_IsReversed; } set { m_IsReversed = value; } }
		public bool IgnoreTimeScale { get { return m_IgnoreTimeScale; } set { m_IgnoreTimeScale = value; } }
		public float Speed { get { return m_Speed; } set { m_Speed = value; } }

		protected virtual void Start()
		{
			if (m_AutoStart)
			{
				CrossFade();
			}
		}

        protected virtual void OnCrossFadeFinished()
        {
			m_Finished?.Invoke();
        }
		
		private IEnumerator CrossFadeRoutine(Color startingColor, Color targetColor, float speed, bool ignoreTimeScale)
		{
			float timer = 0.0f;

			while (Renderer.GetColor() != targetColor)
			{
				timer += (ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime) * speed;
				Renderer.SetColor(Color.Lerp(startingColor, targetColor, m_Curve.Evaluate(timer)));
				yield return null;
			}

			// For mathematical good measure
			Renderer.SetColor(targetColor);

            OnCrossFadeFinished();

			m_CurrentCrossFadeRoutine = null;
		}

		public void CrossFade(Color startingColor, Color targetColor, float speed, bool reversed = false, bool forced = true, bool ignoreTimeScale = true)
		{
            if (m_CurrentCrossFadeRoutine != null && !forced)
                return;

			if (reversed)
			{
				m_CurrentCrossFadeRoutine = StartCoroutine(CrossFadeRoutine(targetColor, startingColor, speed, ignoreTimeScale));
			}
			else
			{
				m_CurrentCrossFadeRoutine = StartCoroutine(CrossFadeRoutine(startingColor, targetColor, speed, ignoreTimeScale));
			}
		}

		public void CrossFade(Color startingColor, Color targetColor)
		{
			CrossFade(startingColor, targetColor, m_Speed, m_IsReversed, m_IgnoreTimeScale);
		}
		
		public void CrossFade(Color targetColor, bool reversed)
		{
			CrossFade(Renderer.GetColor(), targetColor, m_Speed, reversed, m_IgnoreTimeScale);
		}
		
		public void CrossFade(Color targetColor)
		{
			CrossFade(Renderer.GetColor(), targetColor, m_Speed, m_IsReversed, m_IgnoreTimeScale);
		}

		public void CrossFade()
		{
			CrossFade(Renderer.GetColor(), TargetColor, m_Speed, m_IsReversed, m_IgnoreTimeScale);
		}

		public void CrossFadeReset()
		{
			CrossFade(OriginalColor);
		}
	}
}