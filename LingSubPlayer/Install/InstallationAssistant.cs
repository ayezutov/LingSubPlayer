using System;
using System.IO;
using System.Reflection;
using System.Text;
using LingSubPlayer.Common;
using Splat;
using Squirrel;
using Xceed.Wpf.Toolkit;

// ReSharper disable AccessToDisposedClosure

namespace LingSubPlayer.Install
{
    public class InstallationAssistant
    {
        private readonly Configuration configuration;

        public InstallationAssistant(Configuration configuration)
        {
            this.configuration = configuration;
        }

        class SetupLogLogger : ILogger, IDisposable
    {
        TextWriter inner;
        readonly object gate = 42;
        public LogLevel Level { get; set; }

        public SetupLogLogger()
        {
            try {
                var dir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                var file = Path.Combine(dir, "SetupActions.log");
                inner = new StreamWriter(file, true, Encoding.UTF8);
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

        public void PerformStepsIfRequired()
        {
            try
            {
                using (
                    var mgr = new UpdateManager(configuration.UpdateUrl, configuration.ApplicationPackageName,
                        FrameworkVersion.Net45))
                {
                    using (var logger = new SetupLogLogger {Level = LogLevel.Info})
                    {
                        Locator.CurrentMutable.Register(() => logger, typeof (ILogger));
                        SquirrelAwareApp.HandleEvents(
                            onFirstRun: () => { },
                            onInitialInstall: v =>
                            {
                                mgr.CreateShortcutForThisExe();
                            },
                            onAppUpdate: v =>
                            {
                                mgr.CreateShortcutForThisExe();
                            },
                            onAppUninstall: v =>
                            {
                                mgr.RemoveShortcutForThisExe();
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}