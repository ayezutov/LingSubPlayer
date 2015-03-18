using System;
using LingSubPlayer.Common.Subtitles.Data;
using NUnit.Framework;

namespace LingSubPlayer.Common.Tests.Subtitles.Data
{
    [TestFixture]
    public class VideoSubtitleCollectionTests
    {
        [Test]
        [TestCaseSource("Tests")]
        public void VerifyThatTheRightSubtitleIsReturned(TimeSpan timeSpan, string expected)
        {
            var collection = GetData();

            var text = collection.GetTextFromPosition(timeSpan);

            if (expected == null)
            {
                Assert.That(text, Is.Null);
            }
            else
            {
                Assert.That(text.ToString(), Is.EqualTo(expected));
            }
        }

        private TestCaseData[] Tests()
        {
            return new[]
            {
                new TestCaseData(TimeSpan.FromSeconds(-2), null),
                new TestCaseData(TimeSpan.FromSeconds(0), "A"),
                new TestCaseData(TimeSpan.FromSeconds(2), "A"),
                new TestCaseData(TimeSpan.FromSeconds(6), "B"),
                new TestCaseData(TimeSpan.FromSeconds(9), null),
                new TestCaseData(TimeSpan.FromSeconds(10.001), "C"),
                new TestCaseData(TimeSpan.FromSeconds(11.000), "C"),
                new TestCaseData(TimeSpan.FromSeconds(12.000), null),
            };
        }

        private VideoSubtitleCollection GetData()
        {
            return new VideoSubtitleCollection(new[]
            {
                new VideoSubtitlesRecord
                {
                    StartTime = TimeSpan.Parse("00:00:00.000"),
                    EndTime = TimeSpan.Parse("00:00:05.000"),
                    Value = new FormattedText("A")
                },
                new VideoSubtitlesRecord
                {
                    StartTime = TimeSpan.Parse("00:00:05.001"),
                    EndTime = TimeSpan.Parse("00:00:08.000"),
                    Value = new FormattedText("B")
                },
                new VideoSubtitlesRecord()
                {
                    StartTime = TimeSpan.Parse("00:00:10.001"),
                    EndTime = TimeSpan.Parse("00:00:11.000"),
                    Value = new FormattedText("C")
                }
            });
        }
    }
}