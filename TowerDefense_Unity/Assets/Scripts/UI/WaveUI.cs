using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Game.WaveSystem;

namespace Game.UI
{
	public class WaveUI : MonoBehaviour
	{
		[SerializeField] private TMP_Text _TextRenderer = null;

		private Coroutine _TimerRoutine;

		private WaveManager _WaveManager;

		private void OnEnable()
		{
			_WaveManager = WaveManager.Instance;
			_WaveManager.IntermissionStarted += WaveManager_IntermissionStarted;
			_WaveManager.WaveStarted += WaveManager_WaveStarted;
		}

		private void WaveManager_WaveStarted(in WaveManager sender, in WaveEventArgs payload)
		{
			_TextRenderer.text = $"Wave {payload.Wave.Index + 1}";
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
				_TextRenderer.text = $"Next wave in... {Mathf.Ceil(waveManager.IntermissionTimeLeft).ToString("#")}";
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
	}
}
