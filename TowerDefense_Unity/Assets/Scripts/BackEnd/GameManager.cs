using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game.WaveSystem;
using Game.Entities.Base;
using Game.Entities.Interfaces;
using Game.Entities.EventContainers;

namespace Game
{
	public sealed class GameManager : DDOLMonoBehaviourSingleton<GameManager>
	{
		public event Action<GameOverEventArgs> GameOver;

		private WaveManager _WaveManager;

		[SerializeField] private string _TouchToStartScene = null;
		[SerializeField] private string _GameOverOverlayScene = null;
		[SerializeField] private HomeBase _HomeBase = null;

		protected override void OnEnable()
		{
			base.OnEnable();
			_WaveManager = WaveManager.Instance;
			_WaveManager.WaveEnded += WaveManager_WaveEnded;
			_HomeBase.OnDeath += _HomeBase_OnDeath;
		}

		private void Start()
		{
			SceneManager.LoadScene(_GameOverOverlayScene, LoadSceneMode.Additive);
		}

		private void _HomeBase_OnDeath(in IDamageable sender, in EntityDamaged payload)
		{
			OnGameOver();
		}

		private void WaveManager_WaveEnded(in WaveManager sender, in WaveEventArgs payload)
		{
			if (payload.IsLastWave)
				OnGameOver();
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			if (!_WaveManager)
				return;

			_WaveManager.WaveEnded -= WaveManager_WaveEnded;
		}

		private void OnGameOver(List<Wave> completedWaves = null)
		{
			GameOver?.Invoke(new GameOverEventArgs(completedWaves));
		}

		public void RestartGame()
		{
			SceneManager.LoadScene(_TouchToStartScene);
		}

		public void EndGame(List<Wave> completedWaves = null)
		{
			OnGameOver(completedWaves);
		}
	}
}
