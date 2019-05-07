using System;

namespace Game.Entities.EventContainers
{
	/// <inheritdoc />
	/// <summary>
	/// Class representing a transaction result with
	/// the resource system.
	/// </summary>
	public class TransactionResult : EventArgs
	{
		/// <summary>
        /// Integer representation of the current resource count.
        /// </summary>
		public readonly int ResourceCount;

		/// <summary>
        /// Integer representation of the resource count
        /// before it was changed.
        /// </summary>
		public readonly int PreviousResourceCount;

		/// <summary>
        /// Cost of the transaction.
        /// Positive values are positive.
        /// Negative values are prefixed with -
        /// </summary>
		public readonly int TransactionCost;

		/// <summary>
        /// Boolean indicating whether the transaction has failed
        /// When a transaction is attempted should always await
        /// a response before giving the upgrade.
        /// </summary>
		public readonly bool HasTransactionFailed;

		/// <inheritdoc />
		/// <summary>
		/// Class representing a transaction result with
		/// the resource system.
		/// </summary>
        /// <param name="hasFailed">Boolean indicating whether this transaction has failed.</param>
        /// <param name="resourceCount"> Current count of the resources.</param>
        /// <param name="previousResourceCount"> Count of the resources before transaction</param>
        public TransactionResult(bool hasFailed, int resourceCount, int previousResourceCount)
		{
			HasTransactionFailed = hasFailed;

			ResourceCount = resourceCount;
			PreviousResourceCount = previousResourceCount;

			TransactionCost = previousResourceCount - resourceCount;
		}
	}
}
