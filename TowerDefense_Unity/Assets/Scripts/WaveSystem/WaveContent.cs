using System.Collections;
using System.Collections.Generic;
using Game.Entities.MovingEntities;
using UnityEngine;

namespace Game.WaveSystem
{
	[CreateAssetMenu(fileName = "New Wave", menuName = "Wave")]
	public class WaveContent : ScriptableObject
	{
		[SerializeField] private List<Minion> _Minions = null;

		public List<Minion> Minions { get => _Minions; set => _Minions = value; }
	}
}
