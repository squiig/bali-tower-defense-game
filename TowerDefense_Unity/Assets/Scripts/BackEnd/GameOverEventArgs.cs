using System;
using System.Collections;
using System.Collections.Generic;
using Game.WaveSystem;
using UnityEngine;

namespace Game
{
	public class GameOverEventArgs : EventArgs
	{
		public readonly List<Wave> CompletedWaves;

		public readonly bool HasWon;

		public GameOverEventArgs(List<Wave> completedWaves)
		{
			CompletedWaves = completedWaves ?? WaveManager.Instance.CompletedWaves;
			HasWon = WaveManager.Instance.TotalWaveCount == completedWaves.Count;
		}
	}
}
