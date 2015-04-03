using SystemInterface.IO;
using LingSubPlayer.Wpf.Core.Validators;
using NSubstitute;
using NUnit.Framework;

namespace LingSubPlayer.Wpf.Core.Tests.Validators
{
    [TestFixture]
    public class FileExistsValidationRuleTests
    {
        [Test]
        public void EnsureIsValidIfFileExists()
        {
            var file = Substitute.For<IFile>();
            file.Exists(Arg.Any<string>()).Returns(true);

            var result = new FileExistsValidationRule(file).Validate("", null);

            Assert.That(result.IsValid, Is.True);
            Assert.That(result.ErrorContent, Is.Null);
        }

        [Test]
        public void EnsureIsNotValidIfFileDoesNotExists()
        {
            var file = Substitute.For<IFile>();
            file.Exists(Arg.Any<string>()).Returns(false);

            var result = new FileExistsValidationRule(file).Validate("", null);

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ErrorContent, Is.Not.Null);
        }
    }
}