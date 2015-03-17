using System.Text.RegularExpressions;

namespace LingSubPlayer.Common.Subtitles
{
    public class SrtFileLineDescriptor
    {
        public static readonly Regex BlockBeginningPattern = new Regex(@"^\d+$", RegexOptions.Compiled);
        private readonly string line;

        public SrtFileLineDescriptor(string line)
        {
            this.line = line;
        }

        public bool IsBlockBeginning
        {
            get { return BlockBeginningPattern.IsMatch(line); }
        }

        private SrtFileTimeAndPositionLine timeAndPosition;
        public bool IsTimeAndPosition
        {
            get { return timeAndPosition != null || ((timeAndPosition = SrtFileTimeAndPositionLine.Parse(line)) != null); }
        }

        public SrtFileTimeAndPositionLine TimeAndPositionLine
        {
            get { return timeAndPosition ?? (timeAndPosition = SrtFileTimeAndPositionLine.Parse(line)); }
        }
    }
}