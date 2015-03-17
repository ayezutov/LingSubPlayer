using System;
using System.Runtime.Serialization;

namespace LingSubPlayer.Common.Subtitles
{
    public class SubtitlesParserException : Exception
    {
        public SubtitlesParserException(string message) : base(message)
        {
        }

        public SubtitlesParserException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SubtitlesParserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}