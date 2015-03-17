using System;

namespace LingSubPlayer.Common.Subtitles.Data
{
    public class VideoSubtitlesRecord
    {
        public TimeSpan StartTime { get; set; }
        
        public TimeSpan EndTime { get; set; }

        public FormattedText Value { get; set; }
    }
}