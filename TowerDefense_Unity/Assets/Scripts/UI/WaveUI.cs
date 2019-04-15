using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Game.WaveSystem;

namespace Game.UI
{
	public class WaveUI : MonoBehaviour
	{
		[SerializeField] private CanvasGroup _CanvasGroup = null;
		[SerializeField] private TMP_Text _TextRenderer = null;

		private Coroutine _TimerRoutine;

		private WaveManager _WaveManager;

		private void OnEnable()
		{
			_WaveManager = WaveManager.Instance;
			_WaveManager.IntermissionStarted += WaveManager_IntermissionStarted;
			_WaveManager.WaveStarted += WaveManager_WaveStarted;
		}

		private void Awake()
		{
			if (_CanvasGroup == null)
				_CanvasGroup = GetComponent<CanvasGroup>();
		}

		private void WaveManager_WaveStarted(in WaveManager sender, in WaveEventArgs payload)
		{
			Activate();
			_TextRenderer.text = $"Golf {payload.Wave.Index + 1}";
		}

		private void WaveManager_IntermissionStarted(in WaveManager sender, in IntermissionEventArgs payload)
		{
			if (_TimerRoutine == null)
				_TimerRoutine = StartCoroutine(TimerRoutine(sender));
		}

		private IEnumerator TimerRoutine(WaveManager waveManager)
		{
			while (waveManager.IntermissionTimeLeft > 0)
			{
				_TextRenderer.text = $"Volgende aanval in... {Mathf.Ceil(waveManager.IntermissionTimeLeft).ToString("#")}";
				yield return null;
			}

			_TimerRoutine = null;
		}

		private void OnDisable()
		{
			_WaveManager.IntermissionStarted -= WaveManager_IntermissionStarted;
			_WaveManager.WaveStarted -= WaveManager_WaveStarted;

			if (_TimerRoutine != null)
				StopCoroutine(_TimerRoutine);
		}

		public void Activate()
		{
			_CanvasGroup.alpha = 1f;
		}

		public void Deactivate()
		{
			_CanvasGroup.alpha = 0f;
		}
	}
}
