using UnityEngine.Serialization;
using UnityEngine;

namespace Game.Interaction
{
	public class CameraController : MonoBehaviour
	{
		private Transform _Transform;
		private Transform _CameraTransform;

		[SerializeField] private Vector3 _positionOfClamp;

		[FormerlySerializedAs("sizeOfClamp")]
		[SerializeField] private Vector3 _SizeOfClamp;

		[SerializeField] private float _MoveSensitivity = 1; 
		[SerializeField] private float _ZoomSensitivity = 1;

		private Vector2 _DragVelocity = Vector2.zero;
		private bool _IsDragging;

		public void Start()
		{
			_Transform = transform;
			_CameraTransform = GetComponentInChildren<Camera>().transform;

			TouchInputBehaviour.Instance.OnPinchDelta += Zoom;
			TouchInputBehaviour.Instance.OnDragDelta += Move;
		}

		public void Update()
		{
			if (!_IsDragging)
			{
				_DragVelocity = Vector2.Lerp(_DragVelocity, Vector2.zero, 0.1f);
				transform.Translate(Vector3.left * _DragVelocity.x + Vector3.back * _DragVelocity.y);
			}
			else _IsDragging = false;
		}

		void Move(Vector2 move)
		{
			move *= (0.05f * transform.position.y / 8) * _MoveSensitivity;
			_DragVelocity = move;

			transform.Translate(Vector3.left * move.x + Vector3.back * move.y);
			Clamp();

			_IsDragging = true;
		}

		void Zoom(float delta)
		{
			delta = (delta * 0.2f) * _ZoomSensitivity;
			_Transform.position += _CameraTransform.forward * delta;
			if (CheckClamp())
				_Transform.position -= _CameraTransform.forward * delta;

			Clamp();
		}

		private void Clamp()
		{
			Vector3 pos = _Transform.position;
			Vector3 maxCube = _positionOfClamp + _SizeOfClamp / 2;
			Vector3 minCube = _positionOfClamp - _SizeOfClamp / 2;

			pos.x = Mathf.Clamp(pos.x, minCube.x, maxCube.x);
			pos.y = Mathf.Clamp(pos.y, minCube.y, maxCube.y);
			pos.z = Mathf.Clamp(pos.z, minCube.z, maxCube.z);
			_Transform.position = pos;
		}

		private bool CheckClamp()
		{
			Vector3 pos = _Transform.position;
			Vector3 maxCube = _positionOfClamp + _SizeOfClamp / 2;
			Vector3 minCube = _positionOfClamp - _SizeOfClamp / 2;

			return pos.x < minCube.x || pos.x > maxCube.x
			    || pos.y < minCube.y || pos.y > maxCube.y
			    || pos.z < minCube.z || pos.z > maxCube.z;
		}
		
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireCube(_positionOfClamp, _SizeOfClamp);
		}
	}
}
