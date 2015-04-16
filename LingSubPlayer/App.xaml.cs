using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using LingSubPlayer.Common;
using LingSubPlayer.Common.Logging;
using LingSubPlayer.Install;
using LingSubPlayer.Wpf.Core;
using LingSubPlayer.Wpf.Core.Annotations;
using LingSubPlayer.Wpf.Core.Controllers;
using NLog;
using Squirrel;

[assembly: AssemblyMetadata("SquirrelAwareVersion", "1")]

namespace LingSubPlayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private readonly ILog log;

        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                LogManager.GetCurrentClassLogger()
                    .FatalException("Unhandled exception", args.ExceptionObject as Exception);
            };

            new InstallationAssistant(new Configuration()).PerformStepsIfRequired();
            
            var assemblies = new[]
            {
                typeof(App).Assembly,                   //LingSubPlayer
                typeof(CanBeNullAttribute).Assembly,    //LingSubPlayer.Common
                typeof(Commands).Assembly,              // LingSubPlayer.Wpf.Core
            };

            var containerBuilder = new ContainerBuilder();

            containerBuilder
                .RegisterAssemblyTypes(assemblies)
                .Where(
                    t =>
                        !t.IsAssignableTo(typeof (IView<>)) &&
                        !t.FullName.StartsWith("LingSubPlayer.Wpf.Core.Controllers") &&
                        !t.FullName.StartsWith("LingSubPlayer.Wpf.Core.Annotations"))
                .AsImplementedInterfaces()
                .AsSelf();

            containerBuilder
                .RegisterAssemblyTypes(assemblies)
                .Where(t => t.IsAssignableTo(typeof (IView<>)))
                .AsImplementedInterfaces()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            containerBuilder
                .RegisterAssemblyTypes(assemblies)
                .Where(t => t.FullName.StartsWith("LingSubPlayer.Wpf.Core.Controllers"))
                .AsSelf()
                .InstancePerLifetimeScope();
            
            var container = containerBuilder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                MainController.InitializeApplication();
                
                var app = scope.Resolve<App>();
                app.InitializeComponent();
                var mainController = scope.Resolve<MainController>();

                app.Run(mainController.View as Window);
                
                var updateTask = app.UpdateIfAnyUpdates();
                updateTask.Wait();
            }
        }

        public App(ILog log)
        {
            this.log = log;
        }

        private async Task UpdateIfAnyUpdates()
        {
            var configuration = new Configuration();

            if (!string.IsNullOrEmpty(configuration.UpdateUrl))
            {
                log.Write.Trace("Looking for updates at {0}...", configuration.UpdateUrl);

                using (var mgr = new UpdateManager(configuration.UpdateUrl, configuration.ApplicationPackageName, FrameworkVersion.Net45))
                {
                    try
                    {
                        var info = await mgr.CheckForUpdate();

                        if (info.ReleasesToApply.Any())
                        {
                            log.Write.Trace("Updates were found. Current version is {0}. The following versions to be applied: {1}", info.CurrentlyInstalledVersion.Version.ToString(), string.Join(", ", info.ReleasesToApply.Select(r => r.Version.ToString())));

                            log.Write.Trace("Downloading updates...");

                            await mgr.DownloadReleases(info.ReleasesToApply, i =>
                            {
                                log.Write.Trace("Downloading... {0}%", new[] {i/10});
                            });

                            log.Write.Trace("Successfully downloaded updates.");

                            log.Write.Trace("Applying updates...");

                            await mgr.ApplyReleases(info, i =>
                            {
                                log.Write.Trace("Applying updates... {0}%", new[] {i});
                            });

                            log.Write.Trace("Successfully applied updates.");

                            log.Write.Trace("Creating uninstall shortcuts...");

                            await mgr.CreateUninstallerRegistryEntry();

                            log.Write.Trace("Created uninstall shortcuts.");

                            log.Write.Trace("Update is completed.");
                        }
                        else
                        {
                            log.Write.Trace("No updates were found. Current version is {0}", info.CurrentlyInstalledVersion.Version.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Write.Error("There was an error while checking for updates", ex);
                        return;
                    }
                }
            }
            else
            {
                log.Write.Trace("Update is skipped as there is no update URL specified");
            }

            log.Write.Trace("End!");
        }
    }
}
