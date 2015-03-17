using System;
using System.Globalization;
using System.Text.RegularExpressions;
using LingSubPlayer.Common.Subtitles.Data;

namespace LingSubPlayer.Common.Subtitles
{
    public class SrtFileTimeAndPositionLineParser
    {
        public static readonly Regex TimeAndPositionPattern = new Regex(@"^(?<startTime>\d{1,2}\:\d{1,2}\:\d{1,2}[\.\,]\d+)\s*\-\-\>\s*(?<endTime>\d{1,2}\:\d{1,2}\:\d{1,2}[\.\,]\d+)\s*(?<rest>.*)$", RegexOptions.Compiled);
        private static readonly Regex X1Pattern = new Regex(@"X1:(?<value>\d+)", RegexOptions.Compiled);
        private static readonly Regex X2Pattern = new Regex(@"X2:(?<value>\d+)", RegexOptions.Compiled);
        private static readonly Regex Y1Pattern = new Regex(@"Y1:(?<value>\d+)", RegexOptions.Compiled);
        private static readonly Regex Y2Pattern = new Regex(@"Y2:(?<value>\d+)", RegexOptions.Compiled);

        public SrtFileTimeAndPositionLine Parse(string line)
        {
            var match = TimeAndPositionPattern.Match(line);

            if (!match.Success)
            {
                return null;
            }

            var rest = match.Groups["rest"].Value;

            return new SrtFileTimeAndPositionLine
            (
                TimeSpan.Parse(match.Groups["startTime"].Value.Replace(',', '.'), CultureInfo.InvariantCulture),
                TimeSpan.Parse(match.Groups["endTime"].Value.Replace(',', '.'), CultureInfo.InvariantCulture),
                !string.IsNullOrEmpty(rest)
                    ? new Rect(GetCoord(X1Pattern, rest), GetCoord(X2Pattern, rest), GetCoord(Y1Pattern, rest), GetCoord(Y2Pattern, rest))
                    : new Rect(0, 0, 0, 0)
            );
        }

        private static double GetCoord(Regex pattern, string text)
        {
            var match = pattern.Match(text);

            if (!match.Success)
            {
                return 0;
            }

            double temp;
            return double.TryParse(match.Groups["value"].Value, out temp) ? temp : 0;
        } 
    }
}