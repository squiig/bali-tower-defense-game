using System.Runtime.Remoting.Messaging;
using Game.Entities.EventContainers;

namespace Game.Entities.Interfaces
{
	/// <summary>
    /// Interface representing all damageable entities within the game.
    /// </summary>
	public interface IDamageable
    {
		/// <summary>
        /// Event handler that is fired when an instance is hit.
        /// Gives both this instance and the event class <see cref="EntityDamaged"/>
        /// </summary>
        event TypedEventHandler<IDamageable, EntityDamaged> OnHit;
        event TypedEventHandler<IDamageable, EntityDamaged> OnDeath;

		/// <summary>
        /// Used to get the priority of this damageable.
        /// Targets with lower priority will be targeted last.
        /// </summary>
        /// <returns> The priority to attack this instance of this damageable.</returns>
        int GetPriority();

		/// <summary>
        /// Used when this instance gets hit.
        /// Will reduce the health pool of this entity and
        /// Fire the events.
        /// </summary>
        /// <param name="value"></param>
        void TakeDamage(float value);

		/// <summary>
        /// Used to get the current health of this instance
        /// When first spawned will always indicate max health.
        /// </summary>
        /// <returns> The current health of this instance. </returns>
        float GetHealth();
    }
}
