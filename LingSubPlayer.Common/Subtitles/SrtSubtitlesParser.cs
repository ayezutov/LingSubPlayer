using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace LingSubPlayer.Common.Subtitles
{
    public class SrtSubtitlesParser
    {
        private enum State
        {
            NotStarted,
            BlockBeginningLine,
            TimeAndPositionLine,
            TextLine,
            SeparatorLine,
            Finished
        }

        private State state;

        /// <summary>
        /// Parses SRT (SubRip) file and returns <see cref="VideoSubtitleCollection"/>
        /// </summary>
        /// <param name="data">The string containing subtitles</param>
        /// <returns>Parsed collection of subtitles (<see cref="VideoSubtitleCollection"/>)</returns>
        /// <exception cref="SubtitlesParserException">Is thrown when the format of data is invalid</exception>
        public VideoSubtitleCollection Parse(string data)
        {
            try
            {
                var result = new List<VideoSubtitlesRecord>();

                if (state != State.NotStarted && state != State.Finished)
                {
                    throw new NotSupportedException("Current SRT parser is in use.");
                }

                using (var reader = new StringReader(data))
                {
                    var record = new VideoSubtitlesRecord();
                    StringBuilder textBuffer = new StringBuilder();

                    while (true)
                    {
                        var line = reader.ReadLine();
                        
                        if (string.IsNullOrEmpty(line))
                        {
                            if (state != State.TextLine)
                            {
                                throw new SubtitlesParserException(
                                    string.Format(
                                        "The format of subtitles in block #{0} is invalid: no text was found",
                                        result.Count));
                            }

                            record.Value = FormattedText.Parse(textBuffer.ToString());
                            result.Add(record);
                            state = State.SeparatorLine;

                            if (line == null)
                            {
                                break;
                            }
                            continue;
                        }

                        var lineDescriptor = new SrtFileLineDescriptor(line);

                        switch (state)
                        {
                            case State.NotStarted:
                            case State.SeparatorLine:

                                if (!lineDescriptor.IsBlockBeginning)
                                {
                                    throw new SubtitlesParserException(
                                    string.Format(
                                        "The format of subtitles in block #{0} is invalid: the line following separator (or the first line in file) is not block beginning (counter of subtitles)",
                                        result.Count));
                                }

                                state = State.BlockBeginningLine;
                                break;

                            case State.BlockBeginningLine:

                                if (!lineDescriptor.IsTimeAndPosition)
                                {
                                    throw new SubtitlesParserException(
                                    string.Format(
                                        "The format of subtitles in block #{0} is invalid: the line following separator (or the first line in file) is not block beginning (counter of subtitles)",
                                        result.Count));
                                }

                                var timeAndPositionLine = lineDescriptor.TimeAndPositionLine;
                                record.StartTime = timeAndPositionLine.StartTime;
                                record.EndTime = timeAndPositionLine.EndTime;

                                state  = State.TimeAndPositionLine;

                                break;

                            case State.TimeAndPositionLine:
                            case State.TextLine:

                                textBuffer.Append((textBuffer.Length != 0 ? Environment.NewLine : string.Empty) + line);
                                state = State.TextLine;

                                break;

                        }


                    }
                }

                return new VideoSubtitleCollection(result.ToArray());
            }
            finally
            {
                state = State.Finished;
            }
        }
    }

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

    public class VideoSubtitlesRecord
    {
        public TimeSpan StartTime { get; set; }
        
        public TimeSpan EndTime { get; set; }

        public FormattedText Value { get; set; }
    }

    public class FormattedText
    {
        private readonly string value;

        public FormattedText(string value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value;
        }

        public static FormattedText Parse(string text)
        {
            return new FormattedText(text);
        }
    }
}