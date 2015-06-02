using System;
using System.IO;
using Autofac;
using LingSubPlayer.Business;
using LingSubPlayer.Common;
using LingSubPlayer.DataAccess;
using LingSubPlayer.Wpf.Core;
using LingSubPlayer.Wpf.Core.Annotations;
using LingSubPlayer.Wpf.Core.Controllers;

namespace LingSubPlayer
{
    public class LingSubPlayerAutofacModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            var assemblies = new[]
            {
                typeof(App).Assembly,                   // LingSubPlayer
                typeof(CanBeNullAttribute).Assembly,    // LingSubPlayer.Common
                typeof(Commands).Assembly,              // LingSubPlayer.Wpf.Core
                typeof(IRepositorySettings).Assembly,   // LingSubPlayer.DataAccess
                typeof(RecentFilesService).Assembly,    // LingSubPlayer.Business
                typeof(IView<>).Assembly,               // LingSubPlayer.Presentation
            };

            containerBuilder
                .RegisterAssemblyTypes(assemblies)
                .Where(
                    t =>
                        !t.IsAssignableTo(typeof(IView<>)) &&
                        !IsController(t) &&
                        !t.FullName.StartsWith("LingSubPlayer.Wpf.Core.Annotations"))
                .AsImplementedInterfaces()
                .AsSelf();

            containerBuilder
                .RegisterAssemblyTypes(assemblies)
                .Where(t => t.IsAssignableTo(typeof(IView<>)))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            
            containerBuilder
                .RegisterAssemblyTypes(assemblies)
                .Where(IsController)
                .AsSelf()
                .WithPropertyInjectionFor(p => IsController(p.PropertyType))
                .InstancePerLifetimeScope();

            containerBuilder
                .RegisterInstance(new RepositorySettings(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LingSubPlayer", "data.db")))
                .As<IRepositorySettings>();
        }

        private bool IsController(Type type)
        {
            return type.FullName.StartsWith("LingSubPlayer") && type.FullName.EndsWith("Controller");
        }
    }
}