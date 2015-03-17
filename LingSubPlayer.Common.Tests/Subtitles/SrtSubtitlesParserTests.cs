using System;
using Autofac;
using LingSubPlayer.Common.Subtitles;
using LingSubPlayer.Common.Subtitles.Data;
using LingSubPlayer.Common.Tests.Properties;
using NSubstitute;
using NUnit.Framework;

namespace LingSubPlayer.Common.Tests.Subtitles
{
    [TestFixture]
    public class SrtSubtitlesParserTests
    {
        private readonly CommonTestUtilities utils = new CommonTestUtilities();
        
        [TestFixtureSetUp]
        public void Start()
        {
            var parser = Substitute.For<IFormattedTextParser>();
            parser
                .Parse(Arg.Any<string>())
                .Returns(x => new FormattedText(x.Arg<string>()));

            utils.Register(b =>
            {
                b.RegisterInstance(parser).AsImplementedInterfaces();
            });

            
        }

        [Test]
        public void VerifySimpleParsingWorks()
        {
            const string data = @"5
00:00:13,313 --> 00:00:15,645
Good luck with that, Your Honor.";

            var result = utils.Container.Resolve<SrtSubtitlesParser>().Parse(data);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));

            Assert.That(result.Subtitles[0].StartTime, Is.EqualTo(TimeSpan.FromSeconds(13.313)));
            Assert.That(result.Subtitles[0].EndTime, Is.EqualTo(TimeSpan.FromSeconds(15.645)));
            Assert.That(result.Subtitles[0].Value.ToString(), Is.EqualTo("Good luck with that, Your Honor."));
        }

        [Test]
        public void VerifyMultipleRowsCanBeParsed()
        {
            const string data = @"5
00:00:13,313 --> 00:00:15,645
Good luck with that,
Your Honor.";

            var result = utils.Container.Resolve<SrtSubtitlesParser>().Parse(data);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.Subtitles[0].Value.ToString(), Is.EqualTo("Good luck with that,\r\nYour Honor."));
        }

        [Test]
        public void VerifyMultipleBlocksCanBeParsed()
        {
            const string data = @"5
00:00:13,313 --> 00:00:15,645
First.

6
00:00:23,313 --> 00:00:25,645
Second.";

            var result = utils.Container.Resolve<SrtSubtitlesParser>().Parse(data);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Subtitles[0].StartTime, Is.EqualTo(TimeSpan.FromSeconds(13.313)));
            Assert.That(result.Subtitles[0].EndTime, Is.EqualTo(TimeSpan.FromSeconds(15.645)));
            Assert.That(result.Subtitles[0].Value.ToString(), Is.EqualTo("First."));
            Assert.That(result.Subtitles[1].StartTime, Is.EqualTo(TimeSpan.FromSeconds(23.313)));
            Assert.That(result.Subtitles[1].EndTime, Is.EqualTo(TimeSpan.FromSeconds(25.645)));
            Assert.That(result.Subtitles[1].Value.ToString(), Is.EqualTo("Second."));
        }

        [Test]
        public void VerifyMissingEmptyLineDoesNotCreateAnotherRecord()
        {
            const string data = @"5
00:00:13,313 --> 00:00:15,645
First.
6
00:00:23,313 --> 00:00:25,645
Second.";

            var result = utils.Container.Resolve<SrtSubtitlesParser>().Parse(data);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.Subtitles[0].StartTime, Is.EqualTo(TimeSpan.FromSeconds(13.313)));
            Assert.That(result.Subtitles[0].EndTime, Is.EqualTo(TimeSpan.FromSeconds(15.645)));
            Assert.That(result.Subtitles[0].Value.ToString(), Is.EqualTo(string.Format(@"First.{0}6{0}00:00:23,313 --> 00:00:25,645{0}Second.", Environment.NewLine)));
        }

        [Test]
        public void VerifyMissingTimeResultsInException()
        {
            const string data = @"5
00:00:13,313 --> 00:00:15,645
First.

6
Second.";

            var ex = Assert.Throws<SubtitlesParserException>(()=> utils.Container.Resolve<SrtSubtitlesParser>().Parse(data));

            Assert.That(ex.Message, Contains.Substring("block #2"));
            Assert.That(ex.Message, Contains.Substring("not a supported time notation"));
        }

        [Test]
        public void VerifyMissingBlockBeginningResultsInException()
        {
            const string data = @"5
00:00:13,313 --> 00:00:15,645
First.

00:00:23,313 --> 00:00:25,645
Second.";

            var ex = Assert.Throws<SubtitlesParserException>(()=> utils.Container.Resolve<SrtSubtitlesParser>().Parse(data));

            Assert.That(ex.Message, Contains.Substring("block #2"));
            Assert.That(ex.Message, Contains.Substring("is not block beginning"));
        }

        [Test]
        public void VerifyMissingTextResultsInException()
        {
            const string data = @"5
00:00:13,313 --> 00:00:15,645

6
00:00:23,313 --> 00:00:25,645
Second.";

            var ex = Assert.Throws<SubtitlesParserException>(()=> utils.Container.Resolve<SrtSubtitlesParser>().Parse(data));

            Assert.That(ex.Message, Contains.Substring("block #1"));
            Assert.That(ex.Message, Contains.Substring("no text was found"));
        }
    }
}