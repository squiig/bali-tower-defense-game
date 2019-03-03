using Game.Entities.Interfaces;

namespace Game.Entities.MovingEntities
{
	public class MinionAttack : IAttack
	{
		private readonly AttackEffects _attackEffects = new AttackEffects(20, new[] {StatusEffects.NONE});

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

		/// <inheritdoc />
		/// <summary>
		/// Executes the attack on the damageable given.
		/// </summary>
		public void ExecuteAttack(in IDamageable damageable) => damageable.ApplyOnHitEffects(_attackEffects);
	}
}