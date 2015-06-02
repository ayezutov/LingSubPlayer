using LingSubPlayer.Wpf.Core.Converters;
using NUnit.Framework;

namespace LingSubPlayer.Wpf.Core.Tests.Converters
{
    [TestFixture]
    public class FullPathToFilenameConverterTests
    {
        [Test]
        [TestCase(@"c:\some\path\to\file.ext", "file.ext")]
        [TestCase(@"file.ext", "file.ext")]
        [TestCase(@"d:\file.ext", "file.ext")]
        [TestCase(null, null)]
        public void ConvertPath(string original, string expected)
        {
            var actual = new FullPathToFilenameConverter().Convert(original, typeof(string), null, null);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}