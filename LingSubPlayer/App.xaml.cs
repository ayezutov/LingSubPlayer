using System;
using System.Reflection;
using System.Windows;
using Autofac;
using LingSubPlayer.Common;
using LingSubPlayer.Install;
using LingSubPlayer.Wpf.Core.Controllers;
using NLog;

//[assembly: AssemblyMetadata("SquirrelAwareVersion", "1")]

namespace LingSubPlayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private readonly MainController mainController;

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

            new InstallationAssistant(new Configuration()).PerformInstallationStepsIfRequired();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<LingSubPlayerAutofacModule>();
            var container = containerBuilder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                MainController.InitializeApplication(scope.Resolve<Configuration>());
                var app = scope.Resolve<App>();
                app.InitializeComponent();
                var mainController = scope.Resolve<MainController>();
                app.Run(mainController.View as Window);
            }
        }
    }
}
