using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LingSubPlayer.Common.Subtitles.Data;

namespace LingSubPlayer.Common.Subtitles
{
    public class SrtSubtitlesParser
    {
        private readonly IFormattedTextParser formattedTextParser;

        public SrtSubtitlesParser(IFormattedTextParser formattedTextParser)
        {
            this.formattedTextParser = formattedTextParser;
        }

        private SrtParserState state;

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

                if (state != SrtParserState.NotStarted && state != SrtParserState.Finished)
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
                            if (state != SrtParserState.TextLine)
                            {
                                throw new SubtitlesParserException(
                                    string.Format(
                                        "The format of subtitles in block #{0} is invalid: no text was found",
                                        result.Count + 1));
                            }

                            record.Value = formattedTextParser.Parse(textBuffer.ToString());
                            result.Add(record);
                            state = SrtParserState.SeparatorLine;
                            record = new VideoSubtitlesRecord();
                            textBuffer.Clear();

                            if (line == null)
                            {
                                break;
                            }
                            continue;
                        }

                        var lineDescriptor = new SrtFileLineDescriptor(line);

                        switch (state)
                        {
                            case SrtParserState.NotStarted:
                            case SrtParserState.SeparatorLine:

                                if (!lineDescriptor.IsBlockBeginning)
                                {
                                    throw new SubtitlesParserException(
                                    string.Format(
                                        "The format of subtitles in block #{0} is invalid: the line following separator (or the first line in file) is not block beginning (counter of subtitles)",
                                        result.Count + 1));
                                }

                                state = SrtParserState.BlockBeginningLine;
                                break;

                            case SrtParserState.BlockBeginningLine:

                                if (!lineDescriptor.IsTimeAndPosition)
                                {
                                    throw new SubtitlesParserException(
                                    string.Format(
                                        "The format of subtitles in block #{0} is invalid: the line following block beginning is not a supported time notation",
                                        result.Count + 1));
                                }

                                var timeAndPositionLine = lineDescriptor.TimeAndPositionLine;
                                record.StartTime = timeAndPositionLine.StartTime;
                                record.EndTime = timeAndPositionLine.EndTime;

                                state  = SrtParserState.TimeAndPositionLine;

                                break;

                            case SrtParserState.TimeAndPositionLine:
                            case SrtParserState.TextLine:

                                textBuffer.Append((textBuffer.Length != 0 ? Environment.NewLine : string.Empty) + line);
                                state = SrtParserState.TextLine;

                                break;
                        }
                    }
                }

                return new VideoSubtitleCollection(result.ToArray());
            }
            finally
            {
                state = SrtParserState.Finished;
            }
        }
    }
}