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
		private SplineSystem.SplinePathManager _PathManager;

		public void Start()
		{
			_PathManager = FindObjectOfType<SplineSystem.SplinePathManager>();
			for (int i = 0; i < _PoolObjectCount; i++)
			{
				Minion minionGameObject = Instantiate(_MinionPrefab, transform);
				Add(minionGameObject);
				ActivateObject(x => x != x.IsConducting());
			}
		}

		public override void ActivateObject(Func<Minion, bool> predicate)
		{
			Minion minion = _objects.Where(x => !x.IsConducting()).FirstOrDefault(predicate);
			if (minion == null || _PathManager.SplineCount == 0) return;

			Vector3 startPoint = _PathManager.GetStartedPoints()[UnityEngine.Random.Range(0, _PathManager.SplineCount)];
			minion.transform.position = startPoint;
			minion.Activate();
		}
	}
}
