using Game.Entities.MovingEntities;
using UnityEngine;

namespace Game.WaveSystem
{
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
