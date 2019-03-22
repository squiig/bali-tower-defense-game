using System;
using System.Collections;
using System.Collections.Generic;
using Game.Entities;
using UnityEngine;

namespace Game.WaveSystem
{
	public class WaveManager : MonoBehaviourSingleton<WaveManager>
	{
		public event TypedEventHandler<WaveManager, WaveEndedArgs> WaveEnded;
		public event TypedEventHandler<WaveManager, IntermissionEndedArgs> IntermissionEnded;

		[SerializeField] private List<WaveContent> _Waves = null;
		[SerializeField] private int _IntermissionTime = 20;

		public enum EState
		{
			INACTIVE = 0,
			WAVE,
			INTERMISSION
		}

		private EState _State;

		private Queue<Wave> _WaitingWaves;
		private List<Wave> _ActiveWaves;

		private int _CurrentWaveIndex = 0;
		private float _IntermissionTimer = 0;

		public EState State => _State;

		public int TotalWaveCount => _Waves.Count;
		public int CurrentWaveIndex => _CurrentWaveIndex;
		public float IntermissionTimeLeft => _IntermissionTimer;
		public List<Wave> ActiveWaves => _ActiveWaves;

		protected override void Awake()
		{
			base.Awake();

			InitializeWaveQueue();
		}

		/// <summary>
		/// Takes the wave contents (defined in the inspector), creates a formal wave from them and lets those take a number, so to speak.
		/// </summary>
		private void InitializeWaveQueue()
		{
			_WaitingWaves = new Queue<Wave>();
			_ActiveWaves = new List<Wave>();

			int len = _Waves.Count;
			for (int i = 0; i < len; i++)
			{
				_WaitingWaves.Enqueue(new Wave(i, _Waves[i]));
			}
		}

		public void StartNextWave()
		{
			Debug.Log("[WaveSystem] Starting next wave...");

			if (_WaitingWaves.Count <= 0)
			{
				Debug.LogWarning("[WaveSystem] No more waves to start, aborting the attempt!");
				return;
			}

			Wave nextWave = _WaitingWaves.Dequeue();

			// Attempt to start new wave
			nextWave.HasStarted += OnNextWaveStarted;
			nextWave.Start(this);
		}

		private void StartIntermission()
		{
			StartCoroutine(IntermissionCoroutine());
		}

		private IEnumerator IntermissionCoroutine()
		{
			_State = EState.INTERMISSION;

			_IntermissionTimer = _IntermissionTime;
			while (_IntermissionTimer > 0)
			{
				_IntermissionTimer -= Time.deltaTime;
				yield return null;
			}

			_IntermissionTimer = 0f;

			StartNextWave();
		}

		protected virtual void OnNextWaveStarted(Wave wave)
		{
			// Wave has succesfully started
			wave.HasEnded += OnWaveEnded;
			_ActiveWaves.Add(wave);
			_State = EState.WAVE;

			Debug.Log("<color=lime>[WaveSystem] Succesfully started next wave!</color>");
		}

		protected virtual void OnWaveEnded(Wave wave)
		{
			wave.HasEnded -= OnWaveEnded;
			_ActiveWaves.Remove(wave);

			bool wasLastWave = _WaitingWaves.Count <= 0;

			WaveEndedArgs payload = new WaveEndedArgs(wave, wasLastWave);
			WaveEnded?.Invoke(this, payload);

			if (!wasLastWave)
				StartIntermission();
			else
				_State = EState.INACTIVE;
		}
	}
}
