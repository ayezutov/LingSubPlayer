using System.IO;
using LingSubPlayer.Common.Subtitles.Data;

namespace LingSubPlayer.Common.Subtitles
{
    public interface ISubtitlesParser
    {
        /// <summary>
        /// Parses a subtitle file and returns <see cref="VideoSubtitleCollection"/>
        /// </summary>
        /// <param name="data">The string containing subtitles</param>
        /// <returns>Parsed collection of subtitles (<see cref="VideoSubtitleCollection"/>)</returns>
        /// <exception cref="SubtitlesParserException">Is thrown when the format of data is invalid</exception>
        VideoSubtitleCollection Parse(Stream data);
    }
}