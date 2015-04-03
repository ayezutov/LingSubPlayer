using System;
using NLog;

namespace LingSubPlayer.Common.Logging
{
    public class Log : ILog
    {
        private readonly Logger logger;

        public Log(Logger logger)
        {
            this.logger = logger;
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