using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.WaveSystem
{
	public class WaveEventArgs : EventArgs
	{
		/// <summary>
		/// The wave that ended.
		/// </summary>
		public readonly Wave Wave;

		/// <summary>
		/// If this was the last planned wave.
		/// </summary>
		public readonly bool IsLastWave;
		
		public WaveEventArgs(Wave wave, bool isLastWave)
		{
			Wave = wave;
			IsLastWave = isLastWave;
		}
	}
}
