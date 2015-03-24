using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Autofac;
using Vlc.DotNet.Core;

namespace LingSubPlayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [STAThread]
        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public static void Main()
        {
            VlcContextInitialize();

            

            var containerBuilder = new ContainerBuilder();
            containerBuilder
                .RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .AsImplementedInterfaces()
                .AsSelf();

            var container = containerBuilder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<App>();
                app.InitializeComponent();
                app.Run(scope.Resolve<MainWindow>());
            }
        }

        private static void VlcContextInitialize()
        {
            var vlcBaseDirectory = new DirectoryInfo(Path.Combine(@"..\..\..\vlc", Environment.Is64BitProcess ? "x64" : "x86"));

            VlcContext.LibVlcDllsPath = vlcBaseDirectory.FullName;
            VlcContext.LibVlcPluginsPath = Path.Combine(vlcBaseDirectory.FullName, "plugins");
            VlcContext.StartupOptions.ScreenSaverEnabled = false;
            VlcContext.Initialize();
        }
    }
}
