using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using Game.Entities.Interfaces;
using Game.Entities.MovingEntities;
using UnityEngine;

namespace Game.Entities.Towers
{
	public class ProjectilePool : SceneObjectPool<TowerProjectile, ProjectilePool>
	{
		private const string PROJECTILE_ASSET = "BackEnd/Projectiles/TurretTestProjectile";

		[SerializeField] private GameObject _Projectile;
		[SerializeField] private int _PoolCount = 50;

		private void Start()
		{
			if(_Projectile == null)
				_Projectile = Resources.Load<GameObject>(PROJECTILE_ASSET);

			if (_Projectile.GetComponent<TowerProjectile>() == null)
			{
				Debug.LogError("You're trying to pool projectiles without the projectile script.");
				return;
			}

			for (int i = 0; i < _PoolCount; i++)
			{
				Add(Instantiate(_Projectile, transform).GetComponent<TowerProjectile>());
				this[i].ReleaseOwnership();
			}
		}

		/// <summary>
		/// Activates the first object if any that is not active.
		/// Any poolable object with '<see cref="IPoolable.IsConducting"/>'
		/// set to false will be included.
		/// </summary>
		/// <param name="predicate">any predicate selecting what to enable.</param>
		public override TowerProjectile ActivateObject(Func<TowerProjectile, bool> predicate)
		{
			if(Count == 0)
				Start();

			TowerProjectile obj = _objects.Where(x => !x.IsConducting()).FirstOrDefault(predicate);
			return obj;
		}
	}
}
