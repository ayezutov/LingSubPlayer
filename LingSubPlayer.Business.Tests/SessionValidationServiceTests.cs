using System.Linq.Expressions;
using LingSubPlayer.Common;
using LingSubPlayer.Common.Data;
using NSubstitute;
using NUnit.Framework;

namespace LingSubPlayer.Business.Tests
{
    [TestFixture]
    public class SessionValidationServiceTests
    {
        [TestCaseSource("Data")]
        public void VerifyWhenFileAlwaysExists(SessionData session, bool isValid)
        {
            var fs = Substitute.For<IFileSystem>();
            fs.FileExists("exists").Returns(true);
            fs.FileExists("invalid").Returns(false);

            var service = new SessionValidationService(fs);
            var result = service.Validate(session);

            Assert.That(result.IsValid, Is.EqualTo(isValid));
        }

        public TestCaseData[] Data
        {
            get
            {
                return new[]
                {
                    new TestCaseData(null, false),
                    new TestCaseData(new SessionData(), false),
                    new TestCaseData(new SessionData()
                    {
                        VideoFileName = "exists"
                    }, false),
                    new TestCaseData(new SessionData()
                    {
                        SubtitlesTranslatedFileName = "exists"
                    }, false),
                    new TestCaseData(new SessionData()
                    {
                        SubtitlesOriginalFileName = "exists"
                    }, false),
                    new TestCaseData(new SessionData()
                    {
                        VideoFileName = "exists",
                        SubtitlesOriginalFileName = "exists"
                    }, false),
                    new TestCaseData(new SessionData()
                    {
                        VideoFileName = "exists",
                        SubtitlesTranslatedFileName = "exists"
                    }, false),
                    new TestCaseData(new SessionData()
                    {
                        SubtitlesOriginalFileName = "exists",
                        SubtitlesTranslatedFileName = "exists"
                    }, false),
                    new TestCaseData(new SessionData()
                    {
                        VideoFileName = "exists",
                        SubtitlesOriginalFileName = "exists",
                        SubtitlesTranslatedFileName = "exists"
                    }, true),
                    new TestCaseData(new SessionData()
                    {
                        VideoFileName = "invalid",
                        SubtitlesOriginalFileName = "exists",
                        SubtitlesTranslatedFileName = "exists"
                    }, false),
                    new TestCaseData(new SessionData()
                    {
                        VideoFileName = "exists",
                        SubtitlesOriginalFileName = "invalid",
                        SubtitlesTranslatedFileName = "exists"
                    }, false),
                    new TestCaseData(new SessionData()
                    {
                        VideoFileName = "exists",
                        SubtitlesOriginalFileName = "exists",
                        SubtitlesTranslatedFileName = "invalid"
                    }, false),
                };
            }
        }

    }
}