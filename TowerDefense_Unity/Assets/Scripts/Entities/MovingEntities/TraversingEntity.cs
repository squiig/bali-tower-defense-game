using System;
using System.Linq;
using Game.Entities.EventContainers;
using Game.Entities.Interfaces;
using UnityEngine;

namespace Game.Entities.MovingEntities
{
	public abstract class TraversingEntity : Entity, IDamageable, IAggressor
	{
		protected float Health;

		protected readonly IAttack Attack;

		protected IDamageable TargetIDamageable;

		private readonly int _priority;

		protected TraversingEntity(float maxHealth, int priority, in IAttack attack)
		{
			Health = maxHealth;
			_priority = priority;
			Attack = attack;
		}

		/// <inheritdoc />
		/// <summary>
		/// Event handler that is fired when this instance is hit.
		/// Gives both this instance and the event class <see cref="T:Game.Entities.EventContainers.EntityDamaged" />
		/// </summary>
		public event TypedEventHandler<IDamageable, EntityDamaged> OnHit;

		/// <inheritdoc />
		/// <summary>
		/// Event handler that is fired when this instance is hit.
		/// Gives both this instance and the event class <see cref="T:Game.Entities.EventContainers.EntityDamaged" />
		/// </summary>
		public event TypedEventHandler<IDamageable, EntityDamaged> OnDeath;

		/// <inheritdoc />
		/// <summary>
		/// Returns the position of this unit.
		/// </summary>
		/// <returns>Returns the position of this unit.</returns>
		public Vector3 GetPosition() => GetLocation();

		/// <inheritdoc />
		/// <summary>
		/// Used to get the priority of this damageable.
		/// Targets with lower priority will be targeted last.
		/// </summary>
		/// <returns> The priority to attack this instance of this damageable.</returns>
		public int GetPriority() => _priority;

		/// <inheritdoc />
		/// <summary>
		/// Used when this instance gets hit.
		/// Will apply the contents of the <see cref="T:Game.Entities.OnHitEffects" />
		/// Will fire the events.
		/// </summary>
		/// <param name="onHitEffects"> Class containing data about the attack</param>
		public void ApplyOnHitEffects(in OnHitEffects onHitEffects)
		{
			float previousHealth = Health;
			Health -= onHitEffects.GetDamage();

			EntityDamaged payload = new EntityDamaged(this, Health, previousHealth);
			
			OnHit?.Invoke(this, payload);

			if(Health <= 0)
				OnDeath?.Invoke(this, payload);

			if(onHitEffects.GetStatusEffects().Any(x=> x == StatusEffects.SLOWED))
				throw new NotImplementedException("Have not implemented movement impairing effects yet.");
		}

		/// <inheritdoc />
		/// <summary>
		/// Used to get the current health of this instance
		/// When first spawned will always indicate max health.
		/// </summary>
		/// <returns> The current health of this instance. </returns>
		public float GetHealth() => Health;

		/// <inheritdoc />
		/// <summary>
		/// Forcefully override the target of this entity.
		/// Target is set by the aggressor itself, but can
		/// be forcefully overriden at will.
		/// If target is out of range, this will reset to attack targets in range.
		/// </summary>
		/// <param name="target"> Target to focus upon.</param>
		public void SetTarget(in IDamageable target) => TargetIDamageable = target; 

		/// <inheritdoc />
		/// <summary>
		/// Used to forcefully attack the current target.
		/// If this instance has no target, target is out of
		/// range, or is reloading. This instance will stand idle.
		/// </summary>
		public abstract void ExecuteAttack();

		/// <inheritdoc />
		/// <summary>
		/// Used to get the attack of this IAggressor.
		/// Returns the attack class of this instance.
		/// </summary>
		/// <returns>Returns the attack class of this instance.</returns>
		public IAttack GetAttack() => Attack;
	}
}