using System;
using System.Linq;
using Game.Entities.Interfaces;
using Game.Entities.MovingEntities;
using UnityEngine;

namespace Game.Entities.Towers
{
	[CreateAssetMenu(fileName = "TowerAttack", menuName = "Tower/Tower Attack", order = 1)]
	public class TowerAttack : ScriptableObject, IAttack
	{
		[SerializeField] private GameObject _Projectile;

		[SerializeField] private AttackType _attackType;

		[SerializeField] private float _areaOfEffect = -1.0f;

		[SerializeField] private AttackEffects _attackEffects;

		public AttackType GetAttackType() => _attackType;

		public float GetAreaOfEffect() => _areaOfEffect;

		public float GetDamage() => _attackEffects.GetDamage();

		public void SetAreaOfEffect(float value) => _areaOfEffect = value;

		public void SetDamage(float value) => _attackEffects.SetDamage(value);

		public void ExecuteAttack(in IDamageable damageable, Vector3? position)
		{
			if(!position.HasValue)
				return;

			if (damageable == null && _areaOfEffect > 0)
			{
				AreaAttack(Allegiance.FRIENDLY, position.Value);
				return;
			}

			Instantiate(_Projectile, position.Value, Quaternion.identity);


			damageable?.ApplyOnHitEffects(_attackEffects);
		}

		private void AreaAttack(Allegiance allegiance, Vector3 position)
		{
			RaycastHit[] hit = Physics.SphereCastAll(position, _areaOfEffect, Vector3.forward);

			for (int index = 0; index < hit.Length; index++)
			{
				IDamageable damageable = hit[index].transform.GetComponent<IDamageable>();
				if (damageable == null || damageable.GetAllegiance() == allegiance)
					continue;

				damageable.ApplyOnHitEffects(_attackEffects);
			}
		}
	}
}
