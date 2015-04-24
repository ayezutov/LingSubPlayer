using System;
using System.Reflection;
using LingSubPlayer.Common;
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

        public void PerformInstallationStepsIfRequired()
        {
            try
            {
                using (
                    var mgr = new UpdateManager(configuration.UpdateUrl, configuration.ApplicationPackageName,
                        FrameworkVersion.Net45))
                {
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}