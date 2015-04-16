using System;

namespace LingSubPlayer.Common.Logging
{
    public interface IInternalLog
    {
        void Trace(string message, params object[] args);
        void Error(string message, Exception exception);
    }
}