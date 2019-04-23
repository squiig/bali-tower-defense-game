using System;
using System.Runtime.Serialization;

namespace Game.Audio
{
	[Serializable]
	public class AudioSysException : Exception
	{
		public AudioSysException()
		{
		}

		public AudioSysException(string message) : base(message)
		{
		}

		public AudioSysException(string message, Exception inner) : base(message, inner)
		{
		}

		protected AudioSysException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}
}
