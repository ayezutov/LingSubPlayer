using LingSubPlayer.Common.Subtitles;
using NUnit.Framework;

namespace LingSubPlayer.Common.Tests.Subtitles
{
    [TestFixture]
    public class FormattedTextParserTests
    {
        [TestCaseSource("Data")]
        public void VerifyUnformattedStringRepresentationRemovesTags(string input, string expectedOutput)
        {
            var result = new FormattedTextParser().Parse(input);
            Assert.That(result.ToString(), Is.EqualTo(expectedOutput));
        }

        public TestCaseData[] Data
        {
            get
            {
                return new[]
                {
                    new TestCaseData("Subtitle", "Subtitle"),
                    new TestCaseData("<i>Italics</i>", "Italics"),
                    new TestCaseData("<i>Italics1</i> <i>Italics2</i>", "Italics1 Italics2"),
                    new TestCaseData("<b><i>Bold Italics</i></b>", "Bold Italics"),
                    new TestCaseData("<b>Bold</b> and <i>Italics</i>", "Bold and Italics"),
                    new TestCaseData(
                        @"<b><i>Bold
Italics</i></b>",
                        @"Bold
Italics"),
                    new TestCaseData("<invalid_tag par=5>Invalid Tag</invalid_tag>", "Invalid Tag"),
                    new TestCaseData("<font color=\"#00FF00\" size=\"6\">This is <font size=\"35\">v<font color=\"#000000\">e</font><i>r</i>y</font> difficult to implement</font>", "This is very difficult to implement"),
                    new TestCaseData("Also <font color=\"#00FF00\" size=\"6\">This is <font size=\"35\">v<font color=\"#000000\">e</font><i>r</i>y</font> difficult to implement</font>", "Also This is very difficult to implement"),
                };
            }
        }
    }
}