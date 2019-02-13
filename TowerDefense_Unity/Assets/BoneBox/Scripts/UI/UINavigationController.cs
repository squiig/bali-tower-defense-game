using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BoneBox.UI
{
	public class UINavigationController : MonoBehaviour
	{
#if !(UNITY_IOS || UNITY_ANDROID)
		[SerializeField] private float m_InputAxisDeadzone = 0.1f;

		private GameObject m_LastSelected = null;

		/// <summary>
		/// Selects the closest selectable in the given direction. If there is none, the last selected is re-selected.
		/// </summary>
		/// <param name="dir"></param>
		private void SelectFromLast(Vector2 dir)
		{
			GameObject currentSelected = EventSystem.current.currentSelectedGameObject;
            UnityEngine.UI.Selectable lastSelectable = m_LastSelected.GetComponent<UnityEngine.UI.Selectable>();
            UnityEngine.UI.Selectable closestToLast = lastSelectable.FindSelectable(dir);

			if (closestToLast == null)
			{
				lastSelectable.Select();
				currentSelected = lastSelectable.gameObject;
			}
			else
			{
				closestToLast.Select();
				currentSelected = closestToLast.gameObject;
			}
		}

		void Update()
		{
			GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

			float xIn = Input.GetAxisRaw("Horizontal");
			float yIn = Input.GetAxisRaw("Vertical");
			Vector2 inputDir = new Vector2(xIn, yIn);
			bool verified = inputDir.magnitude > m_InputAxisDeadzone;

			// If the player is trying to continue selecting from null
			if (verified && currentSelected == null && m_LastSelected != null)
			{
				SelectFromLast(inputDir);
			}

			// Stay up to date
			if (currentSelected != m_LastSelected && currentSelected != null)
			{
				m_LastSelected = currentSelected;
			}
		}
#endif
	}
}
