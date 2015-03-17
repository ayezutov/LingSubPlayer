using System;

namespace LingSubPlayer.Common.Subtitles.Data
{
    public class SrtFileTimeAndPositionLine
    {
        public SrtFileTimeAndPositionLine(TimeSpan startTime, TimeSpan endTime, Rect boundingRect)
        {
            StartTime = startTime;
            EndTime = endTime;
            BoundingRect = boundingRect;
        }

        public TimeSpan StartTime { get; private set; }

        public TimeSpan EndTime { get; private set; }

        public Rect BoundingRect { get; private set; }
    }
}