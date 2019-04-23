using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;


namespace Game.Interaction
{
	/// <summary>
	/// Handles touch input
	/// </summary>
	public class TouchInputBehaviour : DDOLMonoBehaviourSingleton<TouchInputBehaviour>
	{
		public event System.Action<Vector2> OnDragDelta;
		public event System.Action OnDragStart;
		public event System.Action OnDragStop;

		public event System.Action<float> OnPinchDelta;
		public event System.Action OnPinchStart;
		public event System.Action OnPinchStop;

		public event System.Action OnDeselect;
		public event System.Action OnTap;

		[SerializeField] private float _PinchThreshold = 2;
		[SerializeField] private float _DragThreshold = 2;
		private TouchInputState _CurrentState = TouchInputState.TAPPING;
		private Camera _CurrentCamera;


#if UNITY_EDITOR
		[SerializeField] private bool _UseMouseInput = true;
		[SerializeField] private bool _UseKeyboardInput = false;

		private Vector2 _PreviousMousePosition;
#endif
		public void Start()
		{
			_CurrentCamera = Camera.main;
		}
		 
		private void Update()
		{
#if UNITY_EDITOR
			if (_UseMouseInput)
				HandleDebugMouse();
			if (_UseKeyboardInput)
				HandleDebugKeyboard();
#endif

			switch (_CurrentState)
			{
				case TouchInputState.TAPPING:
					HandleTapping();
					break;

				case TouchInputState.DRAGGING:
					HandleDragging();
					break;

				case TouchInputState.PINCHING:
					HandlePinching();
					break;
			}
		}

		private void HandleTapping()
		{
			Touch[] touches = Input.touches;
			for (int i = 0; i < touches.Length; i++)
			{
				if (touches[i].phase != TouchPhase.Ended)
					continue;
				OnTap?.Invoke();

				if (!ShootGraphicsRays(Input.mousePosition))
					if (!ShootPhysicsRay(touches[i].position))
						OnDeselect?.Invoke();
			}

			if (touches.Length == 1)
			{
				if (touches[0].deltaPosition.magnitude > _DragThreshold)
				{
					SetCurrentState(TouchInputState.DRAGGING);
				}
			}

			if (touches.Length == 2)
			{
				float delta = InputUtils.GetTouchDistanceDelta(touches[0], touches[1]);

				if (Mathf.Abs(delta) > _PinchThreshold)
				{
					SetCurrentState(TouchInputState.PINCHING);
				}
			}
		}

		public void SetCurrentState(TouchInputState state)
		{
			switch (_CurrentState)
			{
				case TouchInputState.TAPPING:
					break;
				case TouchInputState.DRAGGING:
					OnDragStop?.Invoke();
					break;
				case TouchInputState.PINCHING:
					OnPinchStop?.Invoke();
					break;
			}

			switch (state)
			{
				case TouchInputState.TAPPING:
					break;
				case TouchInputState.DRAGGING:
					OnDragStart?.Invoke();
					break;
				case TouchInputState.PINCHING:
					OnPinchStart?.Invoke();
					break;
			}

			_CurrentState = state;
		}

		private bool ShootGraphicsRays(Vector2 screenPosition)
		{
			PointerEventData data = new PointerEventData(null);
			data.position = screenPosition;

			// heavy? yes, very dynamic? also yes!
			GraphicRaycaster[] graphicRaycaster = FindObjectsOfType<GraphicRaycaster>();

			for (int i = 0; i < graphicRaycaster.Length; i++)
			{
				List<RaycastResult> results = new List<RaycastResult>();
				graphicRaycaster[i].Raycast(data, results);
				if (results.Count > 0)
					return true;
			}
			return false;
		}

		private bool ShootPhysicsRay(Vector2 screenPosition)
		{
			bool success = false;
			Ray ray = _CurrentCamera.ScreenPointToRay(screenPosition);
			if (Physics.Raycast(ray, out RaycastHit hit))
			{
				ITappable[] tappables = hit.collider.GetComponentsInChildren<ITappable>();
				if (tappables.Length > 0)
				{
					success = true;
					foreach (ITappable tappable in tappables)
						tappable.Tapped();
				}
			}

			return success;
		}

		private void HandleDragging()
		{
			Touch[] touches = Input.touches;
			if (touches.Length == 0)
			{
				SetCurrentState(TouchInputState.TAPPING);
			}
			else
			{
				OnDragDelta?.Invoke(touches[0].deltaPosition);
			}

			if (touches.Length == 2)
			{
				float delta = InputUtils.GetTouchDistanceDelta(touches[0], touches[1]);

				if (Mathf.Abs(delta) > _PinchThreshold)
				{
					SetCurrentState(TouchInputState.PINCHING);
				}
			}
		}

		private void HandlePinching()
		{
			Touch[] touches = Input.touches;
			if (touches.Length == 0)
			{
				SetCurrentState(TouchInputState.TAPPING);
			}
			else
			{
				if (touches.Length == 1)
				{
					if (touches[0].deltaPosition.magnitude > _DragThreshold)
					{
						SetCurrentState(TouchInputState.DRAGGING);
					}
				}

				if (touches.Length < 2)
					return;

				OnPinchDelta?.Invoke(-InputUtils.GetTouchDistanceDelta(touches[0], touches[1]));
			}
		}

		private void HandleDebugMouse()
		{
#if UNITY_EDITOR
			if (Input.GetMouseButtonDown(1))
			{
				_PreviousMousePosition = Input.mousePosition;
				OnDragStart?.Invoke();
			}

			if (Input.GetMouseButton(1))
			{
				Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				OnDragDelta?.Invoke((mousePos - _PreviousMousePosition) * 2);
				_PreviousMousePosition = Input.mousePosition;
			}

			if (Input.GetMouseButtonUp(1))
				OnDragStop?.Invoke();

			OnPinchDelta?.Invoke(Input.mouseScrollDelta.y * 45);

			if (Input.GetMouseButtonDown(0))
			{
				OnTap?.Invoke();

				if (!ShootGraphicsRays(Input.mousePosition))
				if (!ShootPhysicsRay(Input.mousePosition))
				{
					OnDeselect?.Invoke();
				}
			}
#endif
		}

		private void HandleDebugKeyboard()
		{
#if UNITY_EDITOR
			Vector2 moveDelta = Vector2.zero;
			moveDelta.x = -Input.GetAxis("Horizontal") * 8;
			moveDelta.y = -Input.GetAxis("Vertical") * 8;


			float scrolldelta = 0;
			if (Input.GetKey(KeyCode.Q))
			{
				scrolldelta -= 0.2f;
			}

			if (Input.GetKey(KeyCode.E))
			{
				scrolldelta += 0.2f;
			}

			OnDragDelta?.Invoke(moveDelta);
			OnPinchDelta?.Invoke(scrolldelta);
#endif
		}
	}

	public enum TouchInputState
	{
		TAPPING,
		DRAGGING,
		PINCHING
	}
}
