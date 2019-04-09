using System;
using System.Collections;
using System.Collections.Generic;
using Game.Entities;
using Game.Entities.MovingEntities;
using Game.Entities.Interfaces;
using Game.Entities.EventContainers;
using UnityEngine;
using Debug = System.Diagnostics.Debug;
using Object = UnityEngine.Object;

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
				_WaveManager.StopCoroutine(_SpawnRoutine);

			if (killRemaining)
			{
				foreach (Minion minion in _ActiveMinions)
					minion.Kill();
			}

			_IsActive = false;
			OnHasEnded();
		}

		private IEnumerator SpawnRoutine()
		{
			_State = EState.SPAWNING;
			int legionCount = _Content.WaveLegionCount;

			SplineSystem.SplinePathManager pathManager = Object.FindObjectOfType<SplineSystem.SplinePathManager>();
			if (pathManager != null)
			{
				for (int i = 0; i < legionCount; i++)
				{
					int minionCount = _Content[i].MinionCount;
					int splineBranchIndex = UnityEngine.Random.Range(0, pathManager.SplineCount);
					for (int j = 0; j < minionCount; j++)
					{
						Minion minion = MinionPoolController.Instance.ActivateMinion(x => x != x.IsConducting(), splineBranchIndex);
						minion.OnDeath += Minion_OnDeath;
						_ActiveMinions.Add(minion);
						yield return new WaitForSeconds(_Content[i].SpawnInterval);
					}
					yield return new WaitForSeconds(_Content.LegionSpawnInterval.GetRandom());
				}
			}

			_State = EState.IDLE;
			if (_ActiveMinions.Count <= 0)
				Stop(false);
		}

		protected virtual void Minion_OnDeath(in IDamageable sender, in EntityDamaged payload)
		{
			sender.OnDeath -= Minion_OnDeath;
			ResourceSystem.Instance.RunTransaction(10);

			if (_ActiveMinions.Count > 0)
				return;

			_State = EState.FINISHED;
			OnHasEnded();
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
