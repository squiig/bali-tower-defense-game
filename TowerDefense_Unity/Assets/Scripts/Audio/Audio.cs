using System;
using System.Collections.Generic;

namespace Game.Audio
{
    /// <summary>
    /// Entrypoint for all audio related things, this should be the only thing you'd have to talk to.
    /// </summary>
    public static class Audio
    {
        private static readonly Lazy<AudioManager> s_AudioManager = new Lazy<AudioManager>(false);

        /// <summary>
        /// Used to send audio events inside of the audio 
        /// </summary>
        /// <param name="audioParams"></param>
        public static void SendEvent(AudioEvent audioParams)
        {
            s_AudioManager.Value.ProcessEvent(audioParams);
        }
    }
}

