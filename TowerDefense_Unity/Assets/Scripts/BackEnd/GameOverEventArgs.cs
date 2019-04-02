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

		public GameOverEventArgs(List<Wave> completedWaves, bool hasWon)
		{
			CompletedWaves = completedWaves;
			HasWon = hasWon;
		}
	}
}
