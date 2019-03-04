using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
	[Serializable]
	public struct MinMaxFloat
	{
		[SerializeField] private float _Min;
		[SerializeField] private float _Max;

		public float Min { get => _Min; set => _Min = value; }
		public float Max { get => _Max; set => _Max = value; }

		public MinMaxFloat(float min, float max)
		{
			_Min = min;
			_Max = max;
		}

		public float GetRandom()
		{
			return Random.Range(Min, Max);
		}
	}

	[Serializable]
	public struct MinMaxInt
	{
		[SerializeField] private int _Min;
		[SerializeField] private int _Max;

		public int Min { get => _Min; set => _Min = value; }
		public int Max { get => _Max; set => _Max = value; }

		public MinMaxInt(int min, int max)
		{
			_Min = min;
			_Max = max;
		}

		public int GetRandom()
		{
			return Random.Range(Min, Max);
		}
	}
}
