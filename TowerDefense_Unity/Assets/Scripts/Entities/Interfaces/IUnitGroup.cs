using UnityEngine;

namespace Game.Entities.Interfaces
{
	/// <summary>
	/// Interface representing groups of units.
	/// </summary>
	public interface IUnitGroup
	{
		/// <summary>
		/// Used to get all damageables of this group.
		/// Will never return null.
		/// </summary>
		/// <returns> Returns all damageables contained within this unit. Never null.</returns>
		IDamageable[] GetDamageables();

		/// <summary>
		/// Used to get all aggressors of this group.
		/// Will never return null.
		/// </summary>
		/// <returns> Returns all agressors contained within this unit. Never null.</returns>
		IAggressor[] GetAggressors();

		/// <summary>
		/// Used to get the entity that leads this unit.
		/// Returns the lead entity, this will be changed upon death.
		/// Null if no entities present.
		/// </summary>
		/// <returns> The entity that leads the instance. Null if no units present. </returns>
		Entity LeadInstance();

		/// <summary>
		/// Used to get the centre point of all entities within this unit.
		/// Will return Vector3.zero if no units are present in this unit.
		/// </summary>
		/// <returns> Centre point of all units, Vector3.zero if no units are present.</returns>
		Vector3 CentrePoint();
	}
}