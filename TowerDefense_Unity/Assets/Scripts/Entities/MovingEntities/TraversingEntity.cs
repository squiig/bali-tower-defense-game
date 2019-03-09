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

		protected IAttack Attack;

		protected Allegiance Allegiance;

		protected IDamageable TargetIDamageable;

		private int _priority;

		private bool _isConducting = false;

		protected void Initialize(float maxHealth, int priority, Allegiance allegiance, in IAttack attack)
		{
			//Deprecated
			//if (!MemoryObjectPool<IDamageable>.Instance.Contains(this))
			//{
			//	MemoryObjectPool<IDamageable>.Instance.Add(this);
			//	OnDeath += (in IDamageable sender, in EntityDamaged payload) => ReleaseOwnership();
            //}

			Allegiance = allegiance;
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
		/// Used to check whether this object is currently in the game
		/// or is only available in the pool.
		/// [TRUE] if in scene, [FALSE] if only available from pool.
		/// </summary>
		/// <returns> [TRUE] if in scene, [FALSE] if only available from pool.</returns>
		public bool IsConducting() => _isConducting;

		/// <inheritdoc />
		/// <summary>
		/// Activate this instance. Called when an object has been requested from
		/// the pool.
		/// </summary>
        public void Activate()
		{
			_isConducting = true;
			SetActive(true);

			OnDeath += (in IDamageable sender, in EntityDamaged payload) => ReleaseOwnership();
		}

		/// <inheritdoc />
		/// <summary>
		/// Releases ownership of this object and returns it to the pool.
		/// </summary>
        public void ReleaseOwnership()
        {
	        SetActive(false);
            _isConducting = false;
        }

		/// <inheritdoc />
		/// <summary>
		/// Returns the allegiance of this unit
		/// </summary>
		/// <returns></returns>
		public Allegiance GetAllegiance() => Allegiance;

        /// <inheritdoc />
        /// <summary>
        /// Returns the GameObject of this instance.
        /// </summary>
        /// <returns>Returns the GameObject of this instance.</returns>
        public GameObject GetEntity() => GetInstance();

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
