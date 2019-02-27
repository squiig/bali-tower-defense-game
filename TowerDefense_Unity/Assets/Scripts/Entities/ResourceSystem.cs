using System;
using Game;
using Game.Entities.EventContainers;
using UnityEngine;

namespace Game.Entities
{
	/// <summary>
    /// Class handling resources for the user.
    /// </summary>
	public class ResourceSystem : Singleton<ResourceSystem>
	{
		/// <summary>
        /// Event fired when a transaction is attempted.
        /// see <see cref="RunTransaction"/> to start a transaction.
        /// Will always be fired even when a transaction has failed.
        /// Gives data payload about the transaction status, and numbers it affected.
        /// </summary>
		public event TypedEventHandler<ResourceSystem, TransactionResult> OnTransaction;

		/// <summary>
        /// Call to start a transaction of resources.
        /// Should always provide a positive number for increasing resources.
        /// Should always provide a negative number for removing resources.
        /// Returns a transaction result at <see cref="OnTransaction"/>
        /// </summary>
        /// <param name="amount"> Amount to give or take to the player.</param>
		public void RunTransaction(int amount)
		{
			OnTransaction?.Invoke(this, new TransactionResult(true, 0,0));
			throw new NotImplementedException("We have not implemented a transaction yet. Transaction will always fail");
		}
    }
}
