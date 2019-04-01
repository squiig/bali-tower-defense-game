using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.WaveSystem;

namespace Game.UI
{
	[RequireComponent(typeof(Button)), DisallowMultipleComponent]
	public class StartWaveButton : MonoBehaviour
	{
	 	[SerializeField] private CanvasGroup _CanvasGroup;

		private Button _Button;

		private void Awake()
		{
			if (_CanvasGroup == null)
				_CanvasGroup = GetComponent<CanvasGroup>();

			_Button = GetComponent<Button>();
		}

		private void OnEnable()
		{
			_Button.onClick.AddListener(OnButtonPressed);
		}

		private void OnDisable()
		{
			_Button.onClick.RemoveListener(OnButtonPressed);
		}

		private void OnButtonPressed()
		{
			Deactivate();
			WaveManager.Instance.StartNextWave();
		}

		private void Deactivate()
		{
			_CanvasGroup.alpha = 0f;
			_Button.interactable = false;
		}
	}
}
