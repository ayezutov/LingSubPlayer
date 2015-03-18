using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LingSubPlayer.Common.Subtitles.Data
{
    public class VideoSubtitleCollection
    {
        private readonly ObservableCollection<VideoSubtitlesRecord> subtitles;

        public VideoSubtitleCollection(VideoSubtitlesRecord[] videoSubtitlesRecords)
        {
            subtitles = new ObservableCollection<VideoSubtitlesRecord>(videoSubtitlesRecords);
        }

        public int Count
        {
            get { return subtitles.Count; }
        }

        public IReadOnlyList<VideoSubtitlesRecord> Subtitles
        {
            get { return subtitles; }
        }

        public FormattedText GetTextFromPosition(TimeSpan position)
        {
            var videoSubtitlesRecord = subtitles.FirstOrDefault(s => (s.StartTime <= position) && (s.EndTime >= position));
            return videoSubtitlesRecord != null ? videoSubtitlesRecord.Value : null;
        }
    }
}