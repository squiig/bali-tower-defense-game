using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BoneBox.FX
{
	public class FloatingEffect : MonoBehaviour
	{
		[SerializeField] private Vector3 m_Vector;
		[SerializeField] private float m_Speed;
		[SerializeField] private float m_Radius;
		[SerializeField] private bool m_StartRandom = true;

		private RectTransform m_RectTransform;
		private Vector3 m_OriginalPos;
		private float m_Timer;

		private void Awake()
		{
			m_RectTransform = this.GetComponent<RectTransform>();

			m_OriginalPos = IsThisUI() ? (Vector3) m_RectTransform.anchoredPosition : this.transform.position;

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

		private void UpdatePos()
		{
			Vector3 step = Mathf.Sin(m_Timer) * m_Radius * m_Vector;

			// Different movement depending on if we're UI or not.
			if (IsThisUI())
			{
				m_RectTransform.anchoredPosition = m_OriginalPos + step;
			}
			else
			{
				this.transform.position = m_OriginalPos + step;
			}
		}

		private bool IsThisUI()
		{
			return m_RectTransform != null;
		}
	}
}