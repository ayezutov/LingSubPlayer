using Autofac;
using LingSubPlayer.Common;
using LingSubPlayer.Wpf.Core;
using LingSubPlayer.Wpf.Core.Annotations;
using LingSubPlayer.Wpf.Core.Controllers;

namespace LingSubPlayer
{
    public class LingSubPlayerAutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            var assemblies = new[]
            {
                typeof(App).Assembly,                   //LingSubPlayer
                typeof(CanBeNullAttribute).Assembly,    //LingSubPlayer.Common
                typeof(Commands).Assembly,              // LingSubPlayer.Wpf.Core
            };

            containerBuilder
                .RegisterAssemblyTypes(assemblies)
                .Where(
                    t =>
                        !t.IsAssignableTo(typeof(IView<>)) &&
                        !t.FullName.StartsWith("LingSubPlayer.Wpf.Core.Controllers") &&
                        !t.FullName.StartsWith("LingSubPlayer.Wpf.Core.Annotations"))
                .AsImplementedInterfaces()
                .AsSelf();

            containerBuilder
                .RegisterAssemblyTypes(assemblies)
                .Where(t => t.IsAssignableTo(typeof(IView<>)))
                .AsImplementedInterfaces()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            containerBuilder
                .RegisterAssemblyTypes(assemblies)
                .Where(t => t.FullName.StartsWith("LingSubPlayer.Wpf.Core.Controllers"))
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}