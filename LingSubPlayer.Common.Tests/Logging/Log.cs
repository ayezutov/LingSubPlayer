using System.Threading.Tasks;
using LingSubPlayer.Common.Logging;
using NUnit.Framework;

namespace LingSubPlayer.Common.Tests.Logging
{
    [TestFixture]
    public class LogTests
    {
        public class Inner
        {
            public void WriteToLog(ILog log)
            {
                log.Write.Trace("x");
            }
        }

        [Test]
        public void VerifyLogNameIsCorrect()
        {
            var log = new Log();
            var internalLog = log.Write as InternalLog;

            Assert.That(internalLog.Logger.Name, Is.EqualTo(GetType().FullName));
        }
        
        [Test]
        public void VerifyLogNameIsCorrectForAsyncMethod()
        {
            var log = new Log();

            Async(log);

            var internalLog = log.Write as InternalLog;

            Assert.That(internalLog.Logger.Name, Is.EqualTo(GetType().FullName));
        }

        [Test]
        public void VerifyLogNameForInnerClassIsCorrect()
        {
            var log = new Log();
            new Inner().WriteToLog(log);
            var internalLog = log.Write as InternalLog;

            Assert.That(internalLog.Logger.Name, Is.EqualTo(typeof(Inner).FullName));
        }

        public async Task Async(ILog log)
        {
            log.Write.Trace("X");
        }
    }
}