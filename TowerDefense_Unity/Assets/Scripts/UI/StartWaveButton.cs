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
		private Button _Button;

		private void Awake()
		{
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
			WaveManager.Instance.StartNextWave();
		}
	}
}
