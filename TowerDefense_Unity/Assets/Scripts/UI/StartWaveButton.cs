using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.WaveSystem;
using TMPro;

namespace Game.UI
{
	[RequireComponent(typeof(Button)), DisallowMultipleComponent]
	public class StartWaveButton : MonoBehaviour
	{
	 	[SerializeField] private CanvasGroup _CanvasGroup = null;
		[SerializeField] private TMP_Text _TextRenderer = null;
		[SerializeField] private string _Text = "Start";

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

		public void Activate()
		{
			_CanvasGroup.alpha = 1f;
			_TextRenderer.text = _Text;
			_Button.interactable = true;
		}

		public void Deactivate()
		{
			Audio.Audio.SendEvent(new Audio.AudioEvent(this, Audio.AudioCommands.PLAY, "ui/menu/startwave"));
			_CanvasGroup.alpha = 0f;
			_Button.interactable = false;
		}
	}
}
