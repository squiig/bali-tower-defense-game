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
		private Coroutine _SpawningCoroutine;
		private List<Minion> _ActiveMinions;

		public bool IsActive => _IsActive;
		public bool IsSpawning => _SpawningCoroutine != null;
		public int Index => _Index;
		public WaveContent Content => _Content;
		public List<Minion> ActiveMinions => _ActiveMinions;

		public Wave(int index, WaveContent data)
		{
			_Index = index;
			_Content = data;
		}

		/// <summary>
		/// Use this to start spawning the minions and await their gruesome but ultimate death.
		/// </summary>
		/// <param name="waveManager">The wave manager to run the coroutine on.</param>
		public void Start(WaveManager waveManager)
		{
			_ActiveMinions = new List<Minion>();
			_SpawningCoroutine = waveManager.StartCoroutine(SpawnRoutine());
			_IsActive = true;
			OnHasStarted();
		}

		/// <summary>
		/// Use this to force stop the wave.
		/// </summary>
		/// <param name="killRemaining">Should all the remaining minions belonging to this wave be killed instantly?</param>
		public void Stop(bool killRemaining)
		{
			if (IsSpawning)
			{
				_WaveManager.StopCoroutine(_SpawningCoroutine);
			}

			if (killRemaining)
			{
				foreach (Minion minion in _ActiveMinions)
				{
					minion.ApplyOnHitEffects(new AttackEffects(damage: 9001, null));
				}
			}

			_IsActive = false;

			OnHasEnded();
		}

		private IEnumerator SpawnRoutine()
		{
			int len = _Content.Minions.Count;
			for (int i = 0; i < len; i++)
			{
				Minion minion = MinionPoolController.Instance.ActivateObject(x => x != x.IsConducting());
				minion.OnDeath += Minion_OnDeath;
				_ActiveMinions.Add(minion);
				yield return new WaitForSeconds(_Content.SpawnInterval.GetRandom());
			}
		}

		protected virtual void Minion_OnDeath(in IDamageable sender, in EntityDamaged payload)
		{
			// Reward
			ResourceSystem.Instance.RunTransaction(10);

			// End the wave if there are no minions left
			if (_ActiveMinions.Count <= 0)
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
