using UnityEngine.Serialization;
using UnityEngine;
using System.Collections.Generic;
using Game.Utils;

namespace Game.Interaction
{
	public class CameraController : MonoBehaviour
	{
		private Transform _CameraTransform;

		[FormerlySerializedAs("_positionOfClamp")]
		[SerializeField] private Vector3 _PositionOfClamp;

		[FormerlySerializedAs("sizeOfClamp")]
		[SerializeField] private Vector3 _SizeOfClamp;

		[SerializeField] private float _MoveSensitivity = 1;
		[SerializeField] private float _ZoomSensitivity = 1;


		private Vector2 _MoveVelocity = Vector2.zero;
		private List<Vector2> _MoveVelocitySamples = new List<Vector2>();

		private float _ZoomVelocity = 0f;
		private List<float> _ZoomVelocitySamples = new List<float>();

		private const int MAX_SAMPLE_COUNT = 4;

		private bool _IsDragging = false;
		private bool _IsZooming = false;

		private TouchInputBehaviour _TouchInputBehaviour;

		public void Awake()
		{
			_CameraTransform = GetComponentInChildren<Camera>().transform;
		}

		private void OnEnable()
		{
			_TouchInputBehaviour = TouchInputBehaviour.Instance;
			_TouchInputBehaviour.OnPinchDelta += RecieveZoom;
			_TouchInputBehaviour.OnDragDelta += RecieveMove;
			_TouchInputBehaviour.OnDragStart += _TouchInputBehaviour_OnDragStart;
			_TouchInputBehaviour.OnDragStop += _TouchInputBehaviour_OnDragStop;
			_TouchInputBehaviour.OnPinchStart += _TouchInputBehaviour_OnPinchStart;
			_TouchInputBehaviour.OnPinchStop += _TouchInputBehaviour_OnPinchStop;
		}

		private void _TouchInputBehaviour_OnPinchStop()
		{
			_ZoomVelocity = ArrayUtility.Average(_ZoomVelocitySamples.ToArray());
			_ZoomVelocitySamples.Clear();
			_ZoomVelocitySamples.AddRange(new List<float> { 0, 0, 0, 0 });

			_IsZooming = false;
		}

		private void _TouchInputBehaviour_OnPinchStart()
		{
			_IsZooming = true;
		}

		private void _TouchInputBehaviour_OnDragStop()
		{
			_MoveVelocity = ArrayUtility.GetAverageVec2(_MoveVelocitySamples.ToArray());
			_MoveVelocitySamples.Clear();
			_MoveVelocitySamples.AddRange(new List<Vector2> { Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero });
			_IsDragging = false;
		}

		private void _TouchInputBehaviour_OnDragStart()
		{
			_IsDragging = true;
		}

		private void OnDisable()
		{
			if (_TouchInputBehaviour != null)
			{
				_TouchInputBehaviour.OnPinchDelta -= RecieveZoom;
				_TouchInputBehaviour.OnDragDelta -= RecieveMove;
				_TouchInputBehaviour.OnDragStart -= _TouchInputBehaviour_OnDragStart;
				_TouchInputBehaviour.OnDragStop -= _TouchInputBehaviour_OnDragStop;
				_TouchInputBehaviour.OnPinchStart -= _TouchInputBehaviour_OnPinchStart;
				_TouchInputBehaviour.OnPinchStop -= _TouchInputBehaviour_OnPinchStop;
			}
		}

		public void Update()
		{
			if (!_IsDragging)
			{
				_MoveVelocity = Vector2.Lerp(_MoveVelocity, Vector2.zero, 0.1f);
				Move(_MoveVelocity);
			}

			if (!_IsZooming)
			{
				_ZoomVelocity = Mathf.Lerp(_ZoomVelocity, 0, 0.2f);
				Zoom(_ZoomVelocity);
			}
		}

		private void Move(Vector2 move)
		{
			transform.Translate(Vector3.left * _MoveVelocity.x + Vector3.back * _MoveVelocity.y);
			Clamp();
		}

		private void Zoom(float delta)
		{
			transform.position += _CameraTransform.forward * delta;
			if (CheckClamp())
				transform.position -= _CameraTransform.forward * delta;
			Clamp();
		}

		void RecieveMove(Vector2 move)
		{
			move *= (0.05f * transform.position.y / 8) * _MoveSensitivity;
			_MoveVelocity = move;

			transform.Translate(Vector3.left * move.x + Vector3.back * move.y);
			Clamp();
			_MoveVelocitySamples.Add(move);
			if (_MoveVelocitySamples.Count > MAX_SAMPLE_COUNT)
			{
				_MoveVelocitySamples.RemoveAt(0);
			}
		}

		void RecieveZoom(float delta)
		{
			delta = (delta * 0.2f) * _ZoomSensitivity;
			Zoom(delta);
			_ZoomVelocitySamples.Add(delta);
			if (_ZoomVelocitySamples.Count > MAX_SAMPLE_COUNT)
			{
				_ZoomVelocitySamples.RemoveAt(0);
			}
		}

		private void Clamp()
		{
			Vector3 pos = transform.position;
			Vector3 maxCube = _PositionOfClamp + _SizeOfClamp / 2;
			Vector3 minCube = _PositionOfClamp - _SizeOfClamp / 2;

			pos.x = Mathf.Clamp(pos.x, minCube.x, maxCube.x);
			pos.y = Mathf.Clamp(pos.y, minCube.y, maxCube.y);
			pos.z = Mathf.Clamp(pos.z, minCube.z, maxCube.z);
			transform.position = pos;
		}

		private bool CheckClamp()
		{
			Vector3 pos = transform.position;
			Vector3 maxCube = _PositionOfClamp + _SizeOfClamp / 2;
			Vector3 minCube = _PositionOfClamp - _SizeOfClamp / 2;
			
			return pos.x < minCube.x || pos.x > maxCube.x
				|| pos.y < minCube.y || pos.y > maxCube.y
				|| pos.z < minCube.z || pos.z > maxCube.z;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireCube(_PositionOfClamp, _SizeOfClamp);
		}
	}
}
