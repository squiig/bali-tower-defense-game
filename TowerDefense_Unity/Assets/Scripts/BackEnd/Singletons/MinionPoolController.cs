using UnityEngine;
using Game.Entities.MovingEntities;
using System;
using System.Linq;
using Game.SplineSystem;

namespace Game.Entities
{
	/// <summary>Handles minion pooling.</summary>
	public class MinionPoolController : SceneObjectPool<Minion, MinionPoolController>
	{
		[SerializeField] private Minion _MinionPrefab = null;
		[SerializeField] private int _PoolObjectCount = 20;
		private SplinePathManager _PathManager;

		public void Start()
		{
			_PathManager = SplinePathManager.Instance;
			for (int i = 0; i < _PoolObjectCount; i++)
				Add(Instantiate(_MinionPrefab, transform));
		}

		public Minion ActivateMinion(Func<Minion, bool> predicate, int splineBranchIndex)
		{
			Minion minion = _objects.FirstOrDefault(predicate);

			if (minion == null || _PathManager.SplineCount == 0)
				return null;

			Vector3 startPoint = _PathManager.GetStartedPoints()[splineBranchIndex];
			minion.transform.position = startPoint;
			minion.SplineBranch = _PathManager.GetSplineDataObject(splineBranchIndex);
			minion.Activate();

			return minion;
		}
	}
}
