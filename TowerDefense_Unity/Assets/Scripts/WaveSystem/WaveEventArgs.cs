using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.WaveSystem
{
	public class WaveEventArgs : EventArgs
	{
		public readonly Wave Wave;
		
		public readonly bool IsLastWave;
		
		public WaveEventArgs(Wave wave, bool isLastWave)
		{
			Wave = wave;
			IsLastWave = isLastWave;
		}
	}
}
