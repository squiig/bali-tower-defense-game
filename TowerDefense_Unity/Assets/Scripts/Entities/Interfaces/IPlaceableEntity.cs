using UnityEngine;

namespace Game.Entities.Interfaces
{ 
	/// <summary>
	/// Interface representing entities that can be placed.
	/// </summary>
	public interface IPlaceableEntity
	{
		/// <summary>
		/// Used to construct this instance at given
		/// position and rotation. This entity will regulate
		/// itself after spawning.
		/// </summary>
		/// <param name="position"> Position to place this entity upon.</param>
		/// <param name="rotation"> Rotation to give to this entity. </param>
		void Construct(Vector3 position, Vector3 rotation);

		/// <summary>
		/// Used to destroy this entity.
		/// Ignores health(if applicable) and destroys the entity.
		/// </summary>
		void Destruct();
	}
}