using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Game.Animation
{
	[DisallowMultipleComponent]
	public class BillboardingEffect : MonoBehaviour
	{
		public enum Axis
		{
			UP,
			DOWN,
			LEFT,
			RIGHT,
			FORWARD,
			BACK
		};

		[SerializeField] private Transform _Target = null;
		[SerializeField] private Axis _Axis = Axis.UP;
		[SerializeField] private bool _ReverseFace = false;

		public Transform Target { get => _Target; set => _Target = value; }

		private void Awake()
		{
			if (_Target == null)
				_Target = Camera.main?.transform;
		}

		// Orient the target after all movement is completed this frame to avoid jittering
		private void LateUpdate()
		{
			Vector3 tPos = transform.position + (_Target.transform.rotation * (_ReverseFace ? Vector3.forward : Vector3.back));
			Vector3 tRot = _Target.transform.rotation * GetDirFromAxis(_Axis);

			if (Vector3.Distance(transform.forward, tPos) > 0f)
			{
				transform.LookAt(tPos, tRot);
			}
			
		}
		
		/// <summary>
		/// Returns a direction based upon chosen axis.
		/// </summary>
		/// <param name="refAxis"></param>
		/// <returns></returns>
		public Vector3 GetDirFromAxis(Axis refAxis)
		{
			switch (refAxis)
			{
				case Axis.DOWN:
					return Vector3.down;
				case Axis.FORWARD:
					return Vector3.forward;
				case Axis.BACK:
					return Vector3.back;
				case Axis.LEFT:
					return Vector3.left;
				case Axis.RIGHT:
					return Vector3.right;
			}

			// default is Vector3.up
			return Vector3.up;
		}
	}
}
