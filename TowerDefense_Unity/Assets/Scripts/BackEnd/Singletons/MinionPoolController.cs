using UnityEngine;
using Game.Entities.MovingEntities;
using System;
using System.Linq;

namespace Game.Entities
{
	/// <summary>Handles minion pooling.</summary>
	public class MinionPoolController : SceneObjectPool<Minion>
	{
		[SerializeField] private Minion _MinionPrefab = null;
		[SerializeField] private int _PoolObjectCount = 20;
		[SerializeField] private SplineSystem.SplinePathManager _PathManager;

		public void Start()
		{
			for (int i = 0; i < _PoolObjectCount; i++)
			{
				Minion minionGameObject = Instantiate(_MinionPrefab, transform);
				Add(minionGameObject);
			}
		}

		public override void ActivateObject(Func<Minion, bool> predicate)
		{
			Minion minion = _objects.Where(x => !x.IsConducting()).FirstOrDefault(predicate);


			minion?.Activate();
		}
	}
}
