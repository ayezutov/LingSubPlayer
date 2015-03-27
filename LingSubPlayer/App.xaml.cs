using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Autofac;
using LingSubPlayer.Common;
using LingSubPlayer.Wpf.Core;
using LingSubPlayer.Wpf.Core.Annotations;
using LingSubPlayer.Wpf.Core.Controllers;
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
        public static void Main()
        {
            var assemblies = new[]
            {
                typeof(App).Assembly, //LingSubPlayer
                typeof(CanBeNullAttribute).Assembly, //LingSubPlayer.Common
                typeof(Commands).Assembly, // LingSubPlayer.Wpf.Core
            };

            var containerBuilder = new ContainerBuilder();

            containerBuilder
                .RegisterAssemblyTypes(assemblies)
                .Where(t => !t.IsAssignableTo(typeof(IView<>)) && !t.FullName.StartsWith("LingSubPlayer.Wpf.Core.Controllers"))
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
            }
        }
    }
}
