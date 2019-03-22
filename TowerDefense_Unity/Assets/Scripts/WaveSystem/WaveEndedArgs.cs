using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.WaveSystem
{
	public class WaveEndedArgs : EventArgs
	{
		/// <summary>
		/// The wave that ended.
		/// </summary>
		public readonly Wave Wave;

		/// <summary>
		/// If this was the last planned wave.
		/// </summary>
		public readonly bool WasLastWave;
		
		public WaveEndedArgs(Wave wave, bool wasLastWave)
		{
			Wave = wave;
			WasLastWave = wasLastWave;
		}
	}
}
