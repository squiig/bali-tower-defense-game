using UnityEngine;

namespace Game.Interaction
{
	/// <summary>
	/// Handles touch input
	/// </summary>
	public class TouchInputBehaviour : DDOLMonoBehaviourSingleton<TouchInputBehaviour>
	{
		public event System.Action<Vector2> OnDragDelta;
		public event System.Action<float> OnPinchDelta;
		public event System.Action OnDeselect;
		public event System.Action OnTap;

		[SerializeField] private float _PinchThreshold = 2;
		[SerializeField] private float _DragThreshold = 2;
		private TouchInputState _CurrentState = TouchInputState.TAPPING;
		private Camera _CurrentCamera;

#if UNITY_EDITOR
		private Vector2 _PreviousMousePosition;
#endif
		public void Start()
		{
			_CurrentCamera = Camera.main;
		}

		private void Update()
		{
			HandleDebugMouse();
			HandleDebugKeyboard();

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

				if (!ShootRay(touches[i].position))
					OnDeselect?.Invoke();
			}

			if (touches.Length == 1)
			{
				if (touches[0].deltaPosition.magnitude > _DragThreshold)
				{
					_CurrentState = TouchInputState.DRAGGING;
				}
			}

			if (touches.Length == 2)
			{
				float delta = InputUtils.GetTouchDistanceDelta(touches[0], touches[1]);

				if (Mathf.Abs(delta) > _PinchThreshold)
				{
					_CurrentState = TouchInputState.PINCHING;
				}
			}
		}

		private bool ShootRay(Vector2 screenposition)
		{
			bool success = false;
			Ray ray = _CurrentCamera.ScreenPointToRay(screenposition);
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
				_CurrentState = TouchInputState.TAPPING;
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
					_CurrentState = TouchInputState.PINCHING;
				}
			}
		}

		private void HandlePinching()
		{
			Touch[] touches = Input.touches;
			if (touches.Length == 0)
			{
				_CurrentState = TouchInputState.TAPPING;
			}
			else
			{
				if (touches.Length == 1)
				{
					if (touches[0].deltaPosition.magnitude > _DragThreshold)
					{
						_CurrentState = TouchInputState.DRAGGING;
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
				_PreviousMousePosition = Input.mousePosition;

			if (Input.GetMouseButton(1))
			{
				Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				OnDragDelta?.Invoke(mousePos - _PreviousMousePosition);
				_PreviousMousePosition = Input.mousePosition;
			}

			OnPinchDelta?.Invoke(Input.mouseScrollDelta.y);

			if (Input.GetMouseButton(0))
			{
				if (!ShootRay(Input.mousePosition))
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
