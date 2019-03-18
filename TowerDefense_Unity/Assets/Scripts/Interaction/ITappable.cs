namespace Game.Interaction
{
	/// <summary>
	/// Interface to recieve tap events on a monobehaviour.
	/// NOTE: Needs a collider on the same object or parent object to work.
	/// </summary>
	public interface ITappable
	{
		void Tapped();
	}
}
