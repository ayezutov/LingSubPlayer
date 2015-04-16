using System;
using NLog;

namespace LingSubPlayer.Common.Logging
{
    internal class InternalLog : IInternalLog
    {
        private readonly Logger logger;

        internal Logger Logger
        {
            get { return logger; }
        }

        public InternalLog(string fullTypeName)
        {
            logger = LogManager.GetLogger(fullTypeName);
        }

        public void Trace(string message, params object[] args)
        {
            if (args == null || args.Length == 0)
            {
                logger.Trace(message);
            }
            else
            {
                logger.Trace(message, args);
            }
        }

        public void Error(string message, Exception exception)
        {
            logger.ErrorException(message, exception);
        }
    }
}