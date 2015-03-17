using System.Collections.Generic;
using System.Collections.ObjectModel;

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

        public void Add(VideoSubtitlesRecord record)
        {
        }

        public IReadOnlyList<VideoSubtitlesRecord> Subtitles
        {
            get { return subtitles; }
        }
    }
}