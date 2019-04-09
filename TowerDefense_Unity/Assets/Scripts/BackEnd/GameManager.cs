using System;
using System.Collections;
using System.Collections.Generic;
using Game.WaveSystem;
using UnityEngine;

namespace Game
{
	public sealed class GameManager : DDOLMonoBehaviourSingleton<GameManager>
	{
		public event Action<GameOverEventArgs> GameOver;

		private WaveManager _WaveManager;

		protected override void OnEnable()
		{
			base.OnEnable();
			_WaveManager = WaveManager.Instance;
			_WaveManager.WaveEnded += WaveManager_WaveEnded;
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			if (!_WaveManager)
				return;

			_WaveManager.WaveEnded -= WaveManager_WaveEnded;
		}

		private void WaveManager_WaveEnded(in WaveManager sender, in WaveEventArgs payload)
		{
			if (payload.IsLastWave)
			{
				OnGameOver(sender.CompletedWaves);
			}
		}

		private void OnGameOver(List<Wave> completedWaves)
		{
			bool hasWon = WaveManager.Instance.TotalWaveCount == completedWaves.Count;
			GameOver?.Invoke(new GameOverEventArgs(completedWaves, hasWon));
		}
	}
}
