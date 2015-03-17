using LingSubPlayer.Common.Subtitles;
using NUnit.Framework;

namespace LingSubPlayer.Common.Tests.Subtitles
{
    [TestFixture]
    public class SrtFileLineDescriptorTests
    {
        [TestCase("5", true)]
        [TestCase("a5", false)]
        [TestCase("5 a", false)]
        [TestCase("5 a", false)]
        [TestCase("0x05", false)]
        public void IsMatchToBlockBeginning(string value, bool isMatch)
        {
            var descriptor = new SrtFileLineDescriptor(value);

            Assert.That(descriptor.IsBlockBeginning, Is.EqualTo(isMatch), string.Format("IsBlockBeginning is recognized invalidly for '{0}'. Expected: {1}", value, isMatch));
        }

        [TestCase("00:00:10,500 --> 00:00:13,000", true)]
        [TestCase("00:00:10.500 --> 00:00:13.000", true, Description = "Point as decimal separator")]
        [TestCase("00:00:10,500 --> 00:00:13,000   X1:53 X2:303 Y1:438 Y2:453", true)]
        [TestCase("00:0A:10,500 --> 00:00:13,000", false)]
        [TestCase("00:00:10:500 --> 00:00:13:000", false)]
        [TestCase("AAAAAA", false)]
        public void IsMatchToTimeAndPosition(string value, bool isMatch)
        {
            var descriptor = new SrtFileLineDescriptor(value);

            Assert.That(descriptor.IsTimeAndPosition, Is.EqualTo(isMatch), string.Format("IsTimeAndPosition is recognized invalidly for '{0}'. Expected: {1}", value, isMatch));
        }
    }
}