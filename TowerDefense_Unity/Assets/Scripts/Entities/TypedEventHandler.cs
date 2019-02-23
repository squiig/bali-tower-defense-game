using System;

namespace Game.Entities
{
	/// <summary>
	/// Used to convey information in a strong typed context.
	/// In order to prevent boxing and casting.
	/// </summary>
	/// <typeparam name="TSender">Represents the sender, the class that fired this event</typeparam>
	/// <typeparam name="TPayload">Represents the data given to listeners when fired.</typeparam>
	/// <param name="sender"> The class that fired this event. </param>
	/// <param name="payload"> The data to be given to listeners. </param>
	public delegate void TypedEventHandler<in TSender, in TPayload>(TSender sender, TPayload payload) 
		where TSender : class where TPayload : EventArgs;
}