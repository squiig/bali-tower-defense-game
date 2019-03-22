using System;
using System.Collections;
using System.Collections.Generic;
using Game.Entities;
using Game.Entities.MovingEntities;
using Game.Entities.Interfaces;
using Game.Entities.EventContainers;
using UnityEngine;

namespace Game.WaveSystem
{
	public class Wave
	{
		public event Action<Wave> HasStarted;
		public event Action<Wave> HasEnded;

		private bool _IsActive;
		private WaveManager _WaveManager;
		private int _Index;
		private WaveContent _Content;
		private Coroutine _SpawnRoutine;
		private List<Minion> _ActiveMinions;

		public enum EState
		{
			UNSTARTED = 0,
			SPAWNING,
			IDLE,
			FINISHED
		}

		private EState _State;

		public EState State => _State;

		public bool IsActive => _IsActive;
		public bool IsSpawning => _SpawnRoutine != null;
		public int Index => _Index;
		public WaveContent Content => _Content;
		public List<Minion> ActiveMinions => _ActiveMinions;

		public Wave(int index, WaveContent content)
		{
			_Index = index;
			_Content = content;
			_State = EState.UNSTARTED;
		}

		/// <summary>
		/// Use this to start spawning the minions and await their gruesome but ultimate death.
		/// </summary>
		/// <param name="waveManager">The wave manager to run the coroutine on.</param>
		public void Start(WaveManager waveManager)
		{
			_IsActive = true;
			_ActiveMinions = new List<Minion>();
			OnHasStarted();

			_SpawnRoutine = waveManager.StartCoroutine(SpawnRoutine());
		}

		/// <summary>
		/// Use this to force stop the wave.
		/// </summary>
		/// <param name="killRemaining">Should all the remaining minions belonging to this wave be killed instantly?</param>
		public void Stop(bool killRemaining)
		{
			if (IsSpawning)
			{
				_WaveManager.StopCoroutine(_SpawnRoutine);
			}

			if (killRemaining)
			{
				foreach (Minion minion in _ActiveMinions)
				{
					minion.Kill();
				}
			}

			_IsActive = false;

			OnHasEnded();
		}

		private IEnumerator SpawnRoutine()
		{
			_State = EState.SPAWNING;

			int len = _Content.Minions.Count;
			for (int i = 0; i < len; i++)
			{
				Minion minion = MinionPoolController.Instance.ActivateObject(x => x != x.IsConducting());
				minion.OnDeath += Minion_OnDeath;
				_ActiveMinions.Add(minion);
				yield return new WaitForSeconds(_Content.SpawnInterval.GetRandom());
			}

			_State = EState.IDLE;

			if (_ActiveMinions.Count <= 0)
				Stop(false);
		}

		protected virtual void Minion_OnDeath(in IDamageable sender, in EntityDamaged payload)
		{
			// Reward
			ResourceSystem.Instance.RunTransaction(10);

			// End the wave if there are no minions left
			if (_ActiveMinions.Count <= 0)
			{
				_State = EState.FINISHED;
				OnHasEnded();
			}
		}

		protected virtual void OnHasStarted()
		{
			HasStarted?.Invoke(this);
		}

		protected virtual void OnHasEnded()
		{
			HasEnded?.Invoke(this);
		}
	}
}
