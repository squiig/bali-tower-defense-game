using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BoneBox.FX
{
	[DisallowMultipleComponent]
	public class BillboardingEffect : MonoBehaviour
	{
		[SerializeField]
		private Camera m_TargetCamera;

		public Camera TargetCamera { get { return m_TargetCamera; } set { m_TargetCamera = value; } }

		private void Update()
		{
			Vector3 cameraPos = m_TargetCamera.gameObject.transform.position;
			Vector3 targetDir = cameraPos - this.transform.position;
			float distToTarget = Vector3.Distance(this.transform.forward, targetDir);

			if (!Mathf.Approximately(distToTarget, 0f))
			{
				this.transform.LookAt(cameraPos);
			}
		}
	}
}
