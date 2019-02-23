using System;
using UnityEngine;

namespace Game.Entities.EventContainers
{
	/// <inheritdoc />
	/// <summary>
	/// Class used to convey information about the the instance
	/// after it was it. Contains usage to check state, health and numbers.
	/// </summary>
	public class EntityDamaged : EventArgs
	{
		/// <summary>
		/// Returns the entity that was hit.
		/// </summary>
		public readonly Entity GetEntity;

		/// <summary>
		/// Returns the current health.
		/// Will never go below 0.
		/// </summary>
		public readonly float GetHealth;

		/// <summary>
		/// Returns the health this instance had before
		/// the health was reduced.
		/// </summary>
		public readonly float GetPreviousHealth;

		/// <summary>
		/// Returns the damage inflicted upon this entity.
		/// Will always be a positive number.
		/// </summary>
		public readonly float GetDamageNumber;

		/// <summary>
		/// Returns whether this instance has died.
		/// HasDied = Unit is dead[TRUE], unit is alive [FALSE]
		/// </summary>
		public readonly bool HasDied;
	
		/// <inheritdoc />
		/// <summary>
		/// Construct a new <see cref="EntityDamaged"/> class
		/// And sets variables according to given parameters.
		/// </summary>
		/// <param name="entity">Entity that was hit.</param>
		/// <param name="health"> Current health of this IDamageable.</param>
		/// <param name="previousHealth"> health of this IDamageable before it was hit.</param>
		public EntityDamaged(Entity entity, float health, float previousHealth)
		{
			GetEntity = entity;

			GetHealth = Mathf.Clamp(health, 0, Mathf.Abs(health));
			GetPreviousHealth = previousHealth;

			GetDamageNumber = previousHealth - health;

			HasDied = health <= 0;
		}
	}
}