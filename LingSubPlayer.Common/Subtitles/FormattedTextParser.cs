using LingSubPlayer.Common.Subtitles.Data;

namespace LingSubPlayer.Common.Subtitles
{
    public class FormattedTextParser : IFormattedTextParser
    {
        public FormattedText Parse(string text)
        {
            return new FormattedText(text);
        }
    }
}