///
/// Source: https://forum.unity.com/threads/test-if-ui-element-is-visible-on-screen.276549/
///

using UnityEngine;

namespace BoneBox.UI.Extensions
{
	public static class RectTransformExtensions
	{
		/// <summary>
		/// Determines if this RectTransform is fully visible from the specified camera.
		/// Works by checking if each bounding box corner of this RectTransform is inside the cameras screen space view frustrum.
		/// </summary>
		/// <returns><c>true</c> if is fully visible from the specified camera; otherwise, <c>false</c>.</returns>
		/// <param name="rectTransform">Rect transform.</param>
		/// <param name="camera">Camera.</param>
		public static bool IsFullyVisibleFrom(this RectTransform rectTransform, Camera camera)
		{
			return RectTransformUtility.IsFullyVisibleFrom(rectTransform, camera); // True if all 4 corners are visible
		}

		/// <summary>
		/// Determines if this RectTransform is at least partially visible from the specified camera.
		/// Works by checking if any bounding box corner of this RectTransform is inside the cameras screen space view frustrum.
		/// </summary>
		/// <returns><c>true</c> if is at least partially visible from the specified camera; otherwise, <c>false</c>.</returns>
		/// <param name="rectTransform">Rect transform.</param>
		/// <param name="camera">Camera.</param>
		public static bool IsVisibleFrom(this RectTransform rectTransform, Camera camera)
		{
			return RectTransformUtility.IsVisibleFrom(rectTransform, camera); // True if any corners are visible
		}
	}
}
