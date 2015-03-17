using LingSubPlayer.Common.Subtitles.Data;

namespace LingSubPlayer.Common.Subtitles
{
    public interface IFormattedTextParser
    {
        FormattedText Parse(string text);
    }
}