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
			}
			ActivateObject(x => x != x.IsConducting()); //TODO: Debug code. (spawns in 1 minion from the pool)
		}

		public override void ActivateObject(Func<Minion, bool> predicate)
		{
			Minion minion = _objects.Where(x => !x.IsConducting()).FirstOrDefault(predicate);
			if (minion == null || _PathManager.SplineCount == 0) return;

			int splineBranchIndex = UnityEngine.Random.Range(0, _PathManager.SplineCount);
			Vector3 startPoint = _PathManager.GetStartedPoints()[splineBranchIndex];
			minion.transform.position = startPoint;
			minion.SplineBranch = _PathManager.GetSplineDataObject(splineBranchIndex);
			minion.Activate();
		}
	}
}
