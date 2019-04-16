using UnityEngine;

namespace Game.WaveSystem
{
	[CreateAssetMenu(fileName = "NewWave", menuName = "Wave/Wave")]
	public class WaveContent : ScriptableObject
	{
		[SerializeField] private WaveLegion[] _WaveLegions;
		[SerializeField] private MinMaxFloat _LegionSpawnInterval;

		public WaveLegion this[int i] => _WaveLegions[i];
		public int WaveLegionCount => _WaveLegions.Length;
		public MinMaxFloat LegionSpawnInterval => _LegionSpawnInterval;
	}
}
