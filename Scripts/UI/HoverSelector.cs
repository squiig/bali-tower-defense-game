using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BoneBox.UI
{
	using Core;

	/// <summary>
	/// This component makes sure that any Selectable game object the player hovers over
	/// gets auto-selected in the event system. This has several benefits for the flow
	/// of control when the user likes to use their keyboard a lot.
	/// </summary>
	[DisallowMultipleComponent, RequireComponent(typeof(UnityEngine.UI.Selectable))]
	public class HoverSelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		public void OnPointerEnter(PointerEventData eventData)
		{
			EventSystemUtility.SetSelectedGameObject(gameObject);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			EventSystemUtility.SetNoSelectedGameObject();
		}
	}
}
