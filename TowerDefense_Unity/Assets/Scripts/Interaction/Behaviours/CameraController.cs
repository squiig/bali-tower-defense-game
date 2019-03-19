using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Interaction
{
	public class CameraController : MonoBehaviour
	{
		private Transform _Transform;
		private Transform _CameraTransform;

		[SerializeField]
		private Vector3 _positionOfClamp;

		[SerializeField]
		private Vector3 sizeOfClamp;

		public void Start()
		{
			_Transform = transform;
			_CameraTransform = GetComponentInChildren<Camera>().transform;

			TouchInputBehaviour.Instance.OnPinchDelta += Zoom;
			TouchInputBehaviour.Instance.OnDragDelta += Move;
		}

		void Move(Vector2 move)
		{
			move = move * (0.05f * transform.position.y / 8);
			transform.Translate(Vector3.left * move.x + Vector3.back * move.y);
			Clamp();
		}

		void Zoom(float delta)
		{
			delta = delta * 0.2f;
			_Transform.position += _CameraTransform.forward * delta;
			Clamp();
		}

		private void Clamp()
		{
			Vector3 pos = _Transform.position;
			Vector3 maxCube = _positionOfClamp + sizeOfClamp / 2;
			Vector3 minCube = _positionOfClamp - sizeOfClamp / 2;

			pos.x = Mathf.Clamp(pos.x, minCube.x, maxCube.x);
			pos.y = Mathf.Clamp(pos.y, minCube.y, maxCube.y);
			pos.z = Mathf.Clamp(pos.z, minCube.z, maxCube.z);
			_Transform.position = pos;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawCube(_positionOfClamp, sizeOfClamp);
		}
	}
}
