using UnityEngine;

namespace Game.Entities
{
	/// <inheritdoc />
	/// <summary>
	/// Class representing an entity within the game.
	/// </summary>
	public abstract class Entity : MonoBehaviour
	{
		/// <summary>
		/// Used to get the GameObject of this instance.
		/// When entity is not null, this will never return null.
		/// Returns the GameObject of this instance.
		/// </summary>
		/// <returns>Returns the GameObject of this instance.</returns>
		public GameObject GetInstance() => gameObject;

		/// <summary>
		/// Used to enable and/or disable entities.
		/// SetActive(true) to set active.
		/// SetActive(false) to disable entity.
		/// </summary>
		/// <param name="active"> Boolean representing the GameObject state.</param>
		public void SetActive(bool active) => gameObject.SetActive(active);

        /// <summary>
        /// Returns the location of this entity
        /// </summary>
        /// <returns>Returns the location of this entity</returns>
        public Vector3 GetLocation() => transform.position;
	}
}