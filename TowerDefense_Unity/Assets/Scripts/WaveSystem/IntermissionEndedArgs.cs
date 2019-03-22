using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.WaveSystem
{
	public class IntermissionEndedArgs : EventArgs
	{
		/// <summary>
		/// The wave that ended before this intermission started.
		/// </summary>
		public readonly Wave PreviousWave;

		/// <summary>
		/// The wave that will start after this intermission.
		/// </summary>
		public readonly Wave NextWave;

		public IntermissionEndedArgs(Wave previousWave, Wave nextWave)
		{
			PreviousWave = previousWave;
			NextWave = nextWave;
		}
	}
}
