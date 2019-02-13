using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BoneBox.UI.FX.Base
{
	/// <summary>
	/// Base class for all the visual UI Component effects.
	/// </summary>
	[RequireComponent(typeof(CanvasRenderer))]
	public class GUIFXBase : MonoBehaviour
	{
		private CanvasRenderer m_Renderer;

		public CanvasRenderer Renderer => m_Renderer;

		protected virtual void Awake()
		{
			m_Renderer = GetComponent<CanvasRenderer>();
		}
	}
}
