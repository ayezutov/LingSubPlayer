using HtmlAgilityPack;
using LingSubPlayer.Common.Subtitles.Data;

namespace LingSubPlayer.Common.Subtitles
{
    public class FormattedTextParser : IFormattedTextParser
    {
        public FormattedText Parse(string text)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(text);
            return new FormattedText(doc.DocumentNode.InnerText);
        }
    }
}