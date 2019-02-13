using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BoneBox.Animation.FX
{
	public class FloatingEffect : MonoBehaviour
	{
		public enum ClampMode
		{
			NO_CLAMP = 0,
			POSITIVE_ONLY,
			NEGATIVE_ONLY
		}

		[SerializeField] private Vector3 m_RadiusVector;
		[SerializeField] private float m_Speed;
		[SerializeField] private bool m_StartRandom = true;
		[SerializeField] private ClampMode m_ClampMode = 0;

		private RectTransform m_RectTransform;
		private Vector3 m_OriginalPos;
		private float m_Timer;

		public bool IsUI => m_RectTransform != null;

		private void Awake()
		{
			m_RectTransform = this.GetComponent<RectTransform>();

			m_OriginalPos = IsUI ? (Vector3) m_RectTransform.anchoredPosition : this.transform.position;

			if (m_StartRandom)
			{
				m_Timer = Random.value * (Mathf.PI * 2f);
				UpdatePos();
			}
		}

		private void Update()
		{
			m_Timer += Time.deltaTime * m_Speed;

			UpdatePos();
		}

		public void ResetPos()
		{
			if (IsUI) 
			{
				m_RectTransform.anchoredPosition = m_OriginalPos;
			} 
			else 
			{
				this.transform.position = m_OriginalPos;
			}
		}

		private void UpdatePos()
		{
			Vector3 step = Mathf.Sin(m_Timer) * m_RadiusVector;

			if (m_ClampMode == ClampMode.POSITIVE_ONLY) 
			{
				if (step.x < 0f) step.x = 0f;
				if (step.y < 0f) step.y = 0f;
				if (step.z < 0f) step.z = 0f;
			}
			else if (m_ClampMode == ClampMode.NEGATIVE_ONLY) 
			{
				if (step.x > 0f) step.x = 0f;
				if (step.y > 0f) step.y = 0f;
				if (step.z > 0f) step.z = 0f;
			}

			// Different movement depending on if we're UI or not.
			if (IsUI)
			{
				m_RectTransform.anchoredPosition = m_OriginalPos + step;
			}
			else
			{
				this.transform.position = m_OriginalPos + step;
			}
		}
	}
}