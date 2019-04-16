using UnityEngine;

namespace Game.Audio
{
	/// <summary>
	/// Sets transform equal to other transform
	/// </summary>
	public class TransformFollower
	{
		private Transform _Transform;
		private Transform _TargetTransform; 
		private UnityCallbackBehaviour _CallbackBehaviour;

		public TransformFollower(UnityCallbackBehaviour callbackBehaviour, Transform source)
		{
			_Transform = source;
			_CallbackBehaviour = callbackBehaviour;
		}

		public void StartFollowing(Transform transform)
		{
			_TargetTransform = transform;

			// Ensures we're only subscribed once.
			_CallbackBehaviour.OnLateUpdate -= OnLateUpdate;
			_CallbackBehaviour.OnLateUpdate += OnLateUpdate;
		}

		public void StopFollowing()
		{
			_CallbackBehaviour.OnLateUpdate -= OnLateUpdate;
			_TargetTransform = null;
		}

		private void OnLateUpdate()
		{
			// Stop following if object isn't active
			if (_Transform == null || !_TargetTransform.gameObject.activeInHierarchy)
			{
				StopFollowing();
				return;
			}

			_Transform.position = _TargetTransform.position;
		}
	}
}
