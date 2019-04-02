using Game.Entities.MovingEntities;
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

	[CreateAssetMenu(fileName = "NewLegion", menuName = "Wave/Legion")]
	public class WaveLegion : ScriptableObject
	{
		[SerializeField] private Minion[] _Minions;
		[SerializeField] float _SpawnInterval;

		public Minion this[int i] => _Minions[i];
		public int MinionCount => _Minions.Length;
		public float SpawnInterval => _SpawnInterval;
	}
}
