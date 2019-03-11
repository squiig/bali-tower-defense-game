using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.WaveSystem
{
	public class WaveSystem : MonoBehaviour
	{
		public event Action<Wave> NextWaveStarted;
		public event Action<Wave> LastWaveEnded;

		[SerializeField] private List<WaveContent> _Waves = null;

		private Queue<Wave> _WaitingWaves;
		private List<Wave> _ActiveWaves;

		private int _CurrentWaveIndex = 0;

		public int TotalWaveCount => _Waves.Count;
		public int CurrentWaveIndex => _CurrentWaveIndex;
		public List<Wave> ActiveWaves => _ActiveWaves;

		private void Awake()
		{
			InitializeWaveQueue();
		}

		/// <summary>
		/// Takes the wave contents (defined in the inspector), creates a formal wave from them and lets those take a number, so to speak.
		/// </summary>
		private void InitializeWaveQueue()
		{
			_WaitingWaves = new Queue<Wave>();

			int len = _Waves.Count;
			for (int i = 0; i < len; i++)
			{
				_WaitingWaves.Enqueue(new Wave(i, _Waves[i]));
			}
		}

		public void StartNextWave()
		{
			if (_WaitingWaves.Count <= 0)
			{
				Debug.Log("[WaveSystem] No more waves to start, aborting the attempt!");
				return;
			}

			Wave nextWave = _WaitingWaves.Dequeue();

			// Attempt to start new wave
			nextWave.WaveStarted += OnNextWaveStarted;
			nextWave.Start(this);
		}

		protected virtual void OnNextWaveStarted(Wave wave)
		{
			// Wave has succesfully started
			wave.WaveStoppedOrEnded += OnWaveStoppedOrEnded;
			_ActiveWaves.Add(wave);
			NextWaveStarted?.Invoke(wave);
		}

		protected virtual void OnWaveStoppedOrEnded(Wave wave)
		{
			wave.WaveStoppedOrEnded -= OnWaveStoppedOrEnded;
			_ActiveWaves.Remove(wave);

			if (_WaitingWaves.Count <= 0)
				OnLastWaveEnded(wave);
		}

		protected virtual void OnLastWaveEnded(Wave wave)
		{
			LastWaveEnded?.Invoke(wave);
		}
	}
}
