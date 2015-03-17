using System;
using LingSubPlayer.Common.Subtitles;
using NUnit.Framework;

namespace LingSubPlayer.Common.Tests.Subtitles
{
    [TestFixture]
    public class SrtSubtitlesParserTests
    {
        [Test]
        public void VerifySimpleParsingWorks()
        {
            const string data = @"5
00:00:13,313 --> 00:00:15,645
Good luck with that, Your Honor.";

            var result = new SrtSubtitlesParser().Parse(data);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));

            Assert.That(result.Subtitles[0].StartTime, Is.EqualTo(TimeSpan.FromSeconds(13.313)));
            Assert.That(result.Subtitles[0].EndTime, Is.EqualTo(TimeSpan.FromSeconds(15.645)));
            Assert.That(result.Subtitles[0].Value.ToString(), Is.EqualTo("Good luck with that, Your Honor."));
        }
    }
}