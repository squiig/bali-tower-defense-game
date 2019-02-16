namespace Game.Audio
{
    /// <summary>
    /// Send this object through the Audio class
    /// </summary>
    public class AudioEvent
    {
        public object Context { get; }
        public AudioCommands Command { get; }
        public string Identifier { get; }

        public AudioEvent(object context, AudioCommands command, string identifier = "")
        {
            Command = command;
            Identifier = identifier;
            Context = context;
        }
    }
}