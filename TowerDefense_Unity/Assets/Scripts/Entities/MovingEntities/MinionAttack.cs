using Game.Entities.Interfaces;
using UnityEngine;

namespace Game.Entities.MovingEntities
{
	[CreateAssetMenu(fileName = "MinionAttack", menuName = "Minion/Minion Attack", order = 1)]
	public class MinionAttack : ScriptableObject, IAttack
	{
		[SerializeField] private AttackEffects _AttackEffects;
		[SerializeField] private AttackType _AttackType;

		/// <inheritdoc />
		/// <summary>
		/// Used to gain the attack type of this instance.
		/// </summary>
		/// <returns> The attack type of this instance.</returns>
		public AttackType GetAttackType() => AttackType.SINGLE_TARGET;

		/// <inheritdoc />
		/// <summary>
		/// Gets the area of effect of this attack.
		/// If this instance does not do AoE attacks,
		/// this will always return -1.
		/// </summary>
		/// <returns> The float representing the attacks range. -1 if not applicable.</returns>
		public float GetAreaOfEffect() => -1;

		public float GetDamage() => _AttackEffects.GetDamage();

		/// <inheritdoc />
		/// <summary>
		/// Executes the attack on the damageable given.
		/// </summary>
		public void ExecuteAttack(in IDamageable damageable, Vector3? position) => 
			damageable.ApplyOnHitEffects(_AttackEffects);
	}
}
