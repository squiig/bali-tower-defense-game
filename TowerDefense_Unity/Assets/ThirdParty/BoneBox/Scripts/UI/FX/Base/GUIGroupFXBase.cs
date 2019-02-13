using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BoneBox.UI.FX.Base
{
	/// <summary>
	/// Base class for all the CanvasGroup effects.
	/// </summary>
	[RequireComponent(typeof(CanvasGroup))]
	public class GUIGroupFXBase : MonoBehaviour
	{
		private CanvasGroup m_CanvasGroup;

		public CanvasGroup CanvasGroup => m_CanvasGroup;
        public float Alpha => CanvasGroup.alpha;

        /*
         * Mathf.Approximately doesn't always work properly in WebGL builds, but it's useful otherwise.
         */
#if UNITY_WEBGL
        public bool IsFullyVisible => Alpha > .99f;
        public bool IsFullyInvisible => Alpha < .01f;
#else
        public bool IsFullyVisible => Mathf.Approximately(CanvasGroup.alpha, 1f);
		public bool IsFullyInvisible => Mathf.Approximately(CanvasGroup.alpha, 0f);
#endif

        protected virtual void Awake()
		{
			m_CanvasGroup = GetComponent<CanvasGroup>();
		}
	}
}
