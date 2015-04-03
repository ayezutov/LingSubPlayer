using System;

namespace LingSubPlayer.Common.Logging
{
    public interface ILog
    {
        void Trace(string message, params object[] args);
        void Error(string message, Exception exception);
    }
}