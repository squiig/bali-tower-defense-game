using System.Collections;
using System.Collections.Generic;

namespace BoneBox.Audio
{
    using Data;

    [System.Serializable]
    public struct SFXPlaySettings
    {
        public float Volume { get; set; }
        public MinMaxFloat PitchRange { get; set; }
        public bool Loop { get; set; }

        public SFXPlaySettings(float volume, MinMaxFloat pitchRange, bool loop)
        {
            Volume = volume;
            PitchRange = pitchRange;
            Loop = loop;
        }
    }
}
