using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BoneBox.UI
{
	using Math.Extensions;

	[DisallowMultipleComponent, RequireComponent(typeof(BoxCollider2D), typeof(RectTransform))]
	public class AutoUIColliderScaler : MonoBehaviour
	{
		private BoxCollider2D m_Collider;

        public RectTransform Transform => (RectTransform) transform;
        public bool IsScaledCorrectly => (m_Collider.bounds.size.x == Transform.sizeDelta.x || m_Collider.bounds.size.y == Transform.sizeDelta.y);

        private void Start()
        {
            if (m_Collider == null)
            {
                m_Collider = GetComponent<BoxCollider2D>();
                Correct();
            }
        }

        private void OnValidate()
		{
			if (m_Collider == null)
			{
				m_Collider = GetComponent<BoxCollider2D>();
                Correct();
			}
		}

        public void Correct()
        {
			if (IsScaledCorrectly)
				return;
            
			m_Collider.size = Transform.sizeDelta;
			m_Collider.offset = (Transform.sizeDelta * 0.5f) - Transform.pivot.MultipliedBy(Transform.sizeDelta);
        }
	}
}
