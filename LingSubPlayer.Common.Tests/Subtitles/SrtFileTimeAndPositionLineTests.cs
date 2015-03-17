using System;
using LingSubPlayer.Common.Subtitles;
using NUnit.Framework;

namespace LingSubPlayer.Common.Tests.Subtitles
{
    [TestFixture]
    public class SrtFileTimeAndPositionLineTests
    {
        [TestCaseSource("Data")]
        public void VerifyLineParsing(string value, TimeSpan expectedStartTime, TimeSpan expectedEndTime, Rect expectedRect)
        {
            var result = SrtFileTimeAndPositionLine.Parse(value);

            Assert.That(result.StartTime, Is.EqualTo(expectedStartTime));
            Assert.That(result.EndTime, Is.EqualTo(expectedEndTime));
            Assert.That(result.BoundingRect, Is.EqualTo(expectedRect));
        }

        public TestCaseData[] Data
        {
            get
            {
                return new[]
                {
                    new TestCaseData("00:00:10,500 --> 00:00:13,000", TimeSpan.FromSeconds(10.5),
                        TimeSpan.FromSeconds(13), new Rect(0, 0, 0, 0)),
                    new TestCaseData("00:20:10.500 --> 01:00:13.000",
                        TimeSpan.FromMinutes(20).Add(TimeSpan.FromSeconds(10.5)),
                        TimeSpan.FromHours(1).Add(TimeSpan.FromSeconds(13)), new Rect(0, 0, 0, 0)),
                    new TestCaseData("00:00:00,523 --> 00:00:0,999   X1:53 X2:303 Y1:438 Y2:453",
                        TimeSpan.FromSeconds(0.523), TimeSpan.FromSeconds(0.999), new Rect(53, 303, 438, 453)),
                };
            }
        }
    }
}