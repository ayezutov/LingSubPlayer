using System;
using System.IO;
using System.Text;
using Splat;

namespace LingSubPlayer.Presentation
{
    public class SplatLogger : ILogger, IDisposable
    {
        TextWriter inner;
        readonly object gate = 42;
        public LogLevel Level { get; set; }

        public SplatLogger(string fileName)
        {
            try {
                inner = new StreamWriter(fileName, true, Encoding.UTF8);
            } catch (Exception ex) {
                // Didn't work? Log to stderr
                Console.Error.WriteLine("Couldn't open log file, writing to stderr: " + ex.ToString());
                inner = Console.Error;
            }
        }

        public void Write(string message, LogLevel logLevel)
        {
            if (logLevel < Level) {
                return;
            }

            lock (gate) inner.WriteLine(message);
        }

        public void Dispose()
        {
            lock (gate) {
                inner.Flush();
                inner.Dispose();
            }
        }
    }
}