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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Context the audio belongs to. Think of i as an indentifier for the sound you're about to operate on.</param>
        /// <param name="command">What is the system supposed to do?</param>
        /// <param name="identifier">What sound are we doing this to?</param>
        public AudioEvent(object context, AudioCommands command, string identifier = "")
        {
            Command = command;
            Identifier = identifier;
            Context = context;
        }
    }
}