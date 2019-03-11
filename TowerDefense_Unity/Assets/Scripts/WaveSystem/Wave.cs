using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.WaveSystem
{
	public class Wave
	{
		public event Action<Wave> WaveStarted;
		public event Action<Wave> WaveStoppedOrEnded;

		private WaveSystem _WaveSystem;
		private int _Index;
		private WaveContent _WaveContent;
		private Coroutine _Coroutine;

		public int Index => _Index;
		public WaveContent WaveContent => _WaveContent;
		public bool IsActive => _Coroutine != null;
		
		public Wave(int index, WaveContent data)
		{
			_Index = index;
			_WaveContent = data;
		}

		public void Start(WaveSystem waveSystem)
		{
			_Coroutine = waveSystem.StartCoroutine(WaveRoutine());
			OnWaveStarted();
		}

		public void Stop()
		{
			if (!IsActive)
				return;

			_WaveSystem.StopCoroutine(_Coroutine);
			OnWaveStoppedOrEnded();
		}

		private IEnumerator WaveRoutine()
		{
			int len = _WaveContent.Minions.Count;
			for (int i = 0; i < len; i++)
			{
				// spawn minion
				yield return null;
			}

			OnWaveStoppedOrEnded();
		}

		protected virtual void OnWaveStarted()
		{
			WaveStarted?.Invoke(this);
		}

		protected virtual void OnWaveStoppedOrEnded()
		{
			WaveStoppedOrEnded?.Invoke(this);
		}
	}
}
