using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Game.Animation
{
	[DisallowMultipleComponent]
	public class BillboardingEffect : MonoBehaviour
	{
		[SerializeField]
		private Transform _Target = null;

        [SerializeField]
        private bool
            _IgnoreX = false,
            _IgnoreY = false,
            _IgnoreZ = false;

		public Transform Target { get => _Target; set => _Target = value; }

		private void Start()
		{
			if (_Target == null)
				_Target = Camera.main?.transform;
		}

		// Orient the target after all movement is completed this frame to avoid jittering
		private void LateUpdate()
		{
            if (_IgnoreX && _IgnoreY && _IgnoreZ)
                return;

			Vector3 tPos = _Target.position;
			Quaternion tRot = _Target.rotation;

            if (_IgnoreX)
                tPos.x = transform.position.x;

            if (_IgnoreY)
                tPos.y = transform.position.y;

            if (_IgnoreZ)
                tPos.z = transform.position.z;

            if (Vector3.Distance(transform.forward, tPos) > 0f)
			{
				transform.LookAt(tPos + (tRot * Vector3.forward), tRot * Vector3.up);
			}
		}
	}
}
