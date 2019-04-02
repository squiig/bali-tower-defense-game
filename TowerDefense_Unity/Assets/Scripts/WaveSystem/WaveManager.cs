using System;
using System.Collections;
using System.Collections.Generic;
using Game.Entities;
using UnityEngine;

namespace Game.WaveSystem
{
	public class WaveManager : MonoBehaviourSingleton<WaveManager>
	{
		public event TypedEventHandler<WaveManager, WaveEventArgs> WaveStarted;
		public event TypedEventHandler<WaveManager, WaveEventArgs> WaveEnded;
		public event TypedEventHandler<WaveManager, IntermissionEventArgs> IntermissionStarted;
		public event TypedEventHandler<WaveManager, IntermissionEventArgs> IntermissionEnded;

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
		public Wave NewestWave => ActiveWaves.Count > 0 ? ActiveWaves[ActiveWaves.Count - 1] : null;
		public Wave NextWave => _WaitingWaves.Peek();

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

			if (_Waves == null)
				return;

			int len = _Waves.Count;
			for (int i = 0; i < len; i++)
				_WaitingWaves.Enqueue(new Wave(i, _Waves[i]));
		}

		public void Update()
		{
			if (Input.GetKeyDown(KeyCode.S))
			{
				StartNextWave();
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
			nextWave.HasStarted += OnWaveStarted;
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
			IntermissionEventArgs intermissionEndedArgs = new IntermissionEventArgs(NewestWave, NextWave);
			OnIntermissionStarted(this, intermissionEndedArgs);

			while (_IntermissionTimer > 0)
			{
				_IntermissionTimer -= Time.deltaTime;
				yield return null;
			}

			_IntermissionTimer = 0f;
			OnIntermissionEnded(this, intermissionEndedArgs);
		}

		protected virtual void OnWaveStarted(Wave wave)
		{
			wave.HasEnded += OnWaveEnded;
			_ActiveWaves.Add(wave);
			_State = EState.WAVE;
			bool isLastWave = _WaitingWaves.Count <= 0;

			Debug.Log("<color=lime>[WaveSystem] Succesfully started next wave!</color>");

			WaveEventArgs payload = new WaveEventArgs(wave, isLastWave);
			WaveStarted?.Invoke(this, payload);
		}

		protected virtual void OnWaveEnded(Wave wave)
		{
			wave.HasEnded -= OnWaveEnded;
			_ActiveWaves.Remove(wave);

			bool isLastWave = _WaitingWaves.Count <= 0;

			WaveEventArgs payload = new WaveEventArgs(wave, isLastWave);
			WaveEnded?.Invoke(this, payload);

			if (!isLastWave)
				StartIntermission();
			else
				_State = EState.INACTIVE;
		}

		protected virtual void OnIntermissionStarted(in WaveManager waveManager, in IntermissionEventArgs payload)
		{
			IntermissionStarted?.Invoke(waveManager, payload);
		}

		protected virtual void OnIntermissionEnded(in WaveManager waveManager, in IntermissionEventArgs payload)
		{
			IntermissionEnded?.Invoke(waveManager, payload);
			StartNextWave();
		}
	}
}
