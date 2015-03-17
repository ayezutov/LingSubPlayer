using System;
using Autofac;
using LingSubPlayer.Common.Subtitles;

namespace LingSubPlayer.Common.Tests.Properties
{
    public class CommonTestUtilities
    {
        private static IContainer globalContainer;

        private ILifetimeScope container;
        public ILifetimeScope Container
        {
            get
            {
                EnsureInitialized();
                return container ?? (container = globalContainer.BeginLifetimeScope());
            }
        }

        private static void EnsureInitialized()
        {
            if (globalContainer != null)
            {
                return;
            }

            var builder = new ContainerBuilder();

            builder.RegisterType<SrtSubtitlesParser>();

            globalContainer = builder.Build();
        }

        public void Register(Action<ContainerBuilder> diRegistrations)
        {
            var builder = new ContainerBuilder();
            diRegistrations(builder);
            builder.Update(Container.ComponentRegistry);
        }
    }
}