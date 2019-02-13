///
/// Source: https://bitbucket.org/UnityUIExtensions/unity-ui-extensions/src/4e6f14118ffe5f66ecc9bae3eb8c823922af1442/Scripts/Utilities/UIScrollToSelection.cs?at=master&fileviewer=file-view-default
///
/// Credit zero3growlithe
/// Derived from: http://forum.unity3d.com/threads/scripts-useful-4-6-scripts-collection.264161/page-2#post-2011648
///
/// Edited by: Stan Graafmans
///

/*
 * USAGE:
 * Simply place the script on the ScrollRect that contains the selectable children
 * we'll be scroling to and drag'n'drop the RectTransform of the options "container"
 * that we'll be scrolling.
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("UI/Extensions/UIScrollToSelection")]
	public class UIScrollToSelection : MonoBehaviour
	{
		//*** ENUMS ***//
		public enum ScrollType
		{
			VERTICAL,
			HORIZONTAL,
			BOTH
		}

		//*** ATTRIBUTES ***//
		[Header("[ Settings ]")]
		[SerializeField]
		private ScrollType m_ScrollDirection;
		[SerializeField]
		private float m_ScrollSpeed = 10f;

		/// <summary>
		/// Fraction of an element's height added to the scroll offset to give the element some whitespace after scrolling. Set to 0 for no overflow.
		/// </summary>
		[SerializeField, Tooltip("Fraction of an element's height added to the scroll offset to give the element some whitespace after scrolling. Set to 0 for no overflow.")]
		private float m_OverflowFraction = 0.3f;

		[Header("[ Input ]")]
		[SerializeField]
		private bool m_CancelScrollOnInput = false;
		[SerializeField]
		private List<KeyCode> m_CancelScrollKeycodes = new List<KeyCode>();

        //*** PROPERTIES ***//
        // REFERENCES
        protected RectTransform ScrollRectContentContainer => TargetScrollRect?.content;

        // SETTINGS
        protected ScrollType ScrollDirection => m_ScrollDirection;
        protected float ScrollSpeed => m_ScrollSpeed;

        // INPUT
        protected bool CancelScrollOnInput => m_CancelScrollOnInput;
        protected List<KeyCode> CancelScrollKeycodes => m_CancelScrollKeycodes;

        // CACHED REFERENCES
        protected RectTransform TargetScrollRectViewport { get; set; }
		protected ScrollRect TargetScrollRect { get; set; }

        // SCROLLING
        protected EventSystem CurrentEventSystem => EventSystem.current;
        protected GameObject LastSelectedGameObject { get; set; }
        protected GameObject CurrentSelectedGameObject => EventSystem.current.currentSelectedGameObject;
        protected RectTransform CurrentSelectedRectTransform { get; set; }
		protected bool IsManualScrollingAvailable { get; set; }

		//*** METHODS - PUBLIC ***//


		//*** METHODS - PROTECTED ***//
		protected virtual void Awake()
		{
			TargetScrollRect = GetComponent<ScrollRect>();
			TargetScrollRectViewport = TargetScrollRect.viewport;// .GetComponent<RectTransform>();
		}

		protected virtual void Update()
		{
			UpdateReferences();
			CheckIfScrollingShouldBeLocked();
			ScrollRectToSelection();
		}

		//*** METHODS - PRIVATE ***//
		private void UpdateReferences()
		{
			// update current selected rect transform
			if (CurrentSelectedGameObject != LastSelectedGameObject)
			{
				CurrentSelectedRectTransform = CurrentSelectedGameObject?.GetComponent<RectTransform>();

				// unlock automatic scrolling
				if (CurrentSelectedGameObject != null &&
					CurrentSelectedGameObject.transform.parent == ScrollRectContentContainer.transform)
				{
					IsManualScrollingAvailable = false;
				}
			}

			LastSelectedGameObject = CurrentSelectedGameObject;
		}

		private void CheckIfScrollingShouldBeLocked()
		{
			if (CancelScrollOnInput == false || IsManualScrollingAvailable == true)
			{
				return;
			}

			for (int i = 0; i < CancelScrollKeycodes.Count; i++)
			{
				if (Input.GetKeyDown(CancelScrollKeycodes[i]) == true)
				{
					IsManualScrollingAvailable = true;

					break;
				}
			}
		}

		private void ScrollRectToSelection()
		{
			// check main references
			bool referencesAreIncorrect = (TargetScrollRect == null || ScrollRectContentContainer == null || TargetScrollRectViewport == null);

			if (referencesAreIncorrect == true || IsManualScrollingAvailable == true)
			{
				return;
			}

			RectTransform selection = CurrentSelectedRectTransform;

			// check if scrolling is possible
			if (selection == null || selection.transform.parent != ScrollRectContentContainer.transform)
			{
				return;
			}

			// TODO: Consider refactoring these update functions into one function
			//
			// depending on selected scroll direction move the scroll rect to selection
			switch (ScrollDirection)
			{
				case ScrollType.VERTICAL:
					UpdateVerticalScrollPosition(selection);
					break;
				case ScrollType.HORIZONTAL:
					UpdateHorizontalScrollPosition(selection);
					break;
				case ScrollType.BOTH:
					UpdateVerticalScrollPosition(selection);
					UpdateHorizontalScrollPosition(selection);
					break;
			}
		}

		private float GetScrollOffset(float relativeSelectionPosInverted, float listAnchorPos, float selectionHeight, float maskLength)
		{
			if (relativeSelectionPosInverted < listAnchorPos + (selectionHeight * m_OverflowFraction))
			{
				return listAnchorPos + (selectionHeight * m_OverflowFraction) - relativeSelectionPosInverted;
			}
			else if (relativeSelectionPosInverted + (selectionHeight * (1 + m_OverflowFraction)) > listAnchorPos + maskLength)
			{
				return listAnchorPos + maskLength - (relativeSelectionPosInverted + (selectionHeight * (1 + m_OverflowFraction)));
			}

			return 0;
		}

		private void UpdateVerticalScrollPosition(RectTransform selection)
		{
			float selectionHeight = selection.rect.height;
			float scrollRectHeight = TargetScrollRectViewport.rect.height;
			float listAnchorYPos = ScrollRectContentContainer.anchoredPosition.y;

			float relativeSelectionBottomPosYInverted = -selection.anchoredPosition.y - (selectionHeight * (1 - selection.pivot.y));

			// get the element offset value depending on the cursor move direction
			float offlimitsValue = GetScrollOffset(relativeSelectionBottomPosYInverted, listAnchorYPos, selectionHeight, scrollRectHeight);

			float contentContainerHeight = ScrollRectContentContainer.rect.height;

			if (!Mathf.Approximately(offlimitsValue, 0.0f) && contentContainerHeight != 0)
			{
				TargetScrollRect.verticalNormalizedPosition +=
					(offlimitsValue / contentContainerHeight)
					* Time.unscaledDeltaTime
					* m_ScrollSpeed;
			}
		}

		private void UpdateHorizontalScrollPosition(RectTransform selection)
		{
			float selectionWidth = selection.rect.width;
			float scrollRectWidth = TargetScrollRectViewport.rect.width;
			float listAnchorXPos = ScrollRectContentContainer.anchoredPosition.x;

			float relativeSelectionLeftPosXInverted = -selection.anchoredPosition.x - (selection.rect.width * (1 - selection.pivot.x));

			// get the element offset value depending on the cursor move direction
			float offlimitsValue = GetScrollOffset(relativeSelectionLeftPosXInverted, listAnchorXPos, selectionWidth, scrollRectWidth);

			float contentContainerWidth = ScrollRectContentContainer.rect.width;

			if (!Mathf.Approximately(offlimitsValue, 0.0f) && contentContainerWidth != 0)
			{
				// move the target scroll rect
				TargetScrollRect.horizontalNormalizedPosition +=
					(offlimitsValue / contentContainerWidth)
					* Time.unscaledDeltaTime
					* m_ScrollSpeed;
			}
		}
	}
}
