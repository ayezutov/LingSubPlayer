using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using LingSubPlayer.Common;
using LingSubPlayer.Common.Logging;
using LingSubPlayer.Wpf.Core;
using LingSubPlayer.Wpf.Core.Annotations;
using LingSubPlayer.Wpf.Core.Controllers;
using NLog;
using Squirrel;

namespace LingSubPlayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
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

            var assemblies = new[]
            {
                typeof(App).Assembly, //LingSubPlayer
                typeof(CanBeNullAttribute).Assembly, //LingSubPlayer.Common
                typeof(Commands).Assembly, // LingSubPlayer.Wpf.Core
            };

            var containerBuilder = new ContainerBuilder();

            containerBuilder
                .RegisterAssemblyTypes(assemblies)
                .Where(
                    t =>
                        !t.IsAssignableTo(typeof (IView<>)) &&
                        !t.FullName.StartsWith("LingSubPlayer.Wpf.Core.Controllers"))
                .AsImplementedInterfaces()
                .AsSelf();

            containerBuilder.Register(c => new Log(LogManager.GetCurrentClassLogger())).As<ILog>();

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
            var updateUrl = new Configuration().UpdateUrl;

            if (!string.IsNullOrEmpty(updateUrl))
            {
                log.Trace("Looking for updates at {0}...", updateUrl);

                using (var mgr = new UpdateManager(updateUrl, "LingSubPlayer", FrameworkVersion.Net45))
                {
                    try
                    {
                        var info = await mgr.CheckForUpdate();

                        if (info.ReleasesToApply.Any())
                        {
                            log.Trace("Updates were found. Current version is {0}. The following versions to be applied: {1}", info.CurrentlyInstalledVersion.Version.ToString(), string.Join(", ", info.ReleasesToApply.Select(r => r.Version.ToString())));
                            
                            log.Trace("Downloading updates...");

                            await mgr.DownloadReleases(info.ReleasesToApply, i =>
                            {
                                log.Trace("Downloading... {0}%", i/10);
                            });

                            log.Trace("Successfully downloaded updates.");

                            log.Trace("Applying updates...");

                            await mgr.ApplyReleases(info, i =>
                            {
                                log.Trace("Applying updates... {0}%", i);
                            });

                            log.Trace("Successfully applied updates.");

                            log.Trace("Creating uninstall shortcuts...");

                            await mgr.CreateUninstallerRegistryEntry();

                            log.Trace("Created uninstall shortcuts.");

                            log.Trace("Update is completed.");
                        }
                        else
                        {
                            log.Trace("No updates were found. Current version is {0}", info.CurrentlyInstalledVersion.Version.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("There was an error while checking for updates", ex);
                        return;
                    }
                }
            }
            else
            {
                log.Trace("Update is skipped as there is no update URL specified");
            }

            log.Trace("End!");
        }
    }
}
