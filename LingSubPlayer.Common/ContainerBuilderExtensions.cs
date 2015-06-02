using System;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;

namespace LingSubPlayer
{
    public static class ContainerBuilderExtensions
    {
        public static IRegistrationBuilder<TLimit, ScanningActivatorData, DynamicRegistrationStyle> WithPropertyInjectionFor<TLimit>(
            this IRegistrationBuilder<TLimit, ScanningActivatorData, DynamicRegistrationStyle> registration,
            Func<PropertyInfo, bool> condition)
        {
            registration.OnActivated(args =>
            {
                var instance = args.Instance;
                if (instance == null)
                {
                    return;
                }
                var type = instance.GetType();

                foreach (var property in type.GetProperties())
                {
                    if (condition(property))
                    {
                        property.SetValue(instance, args.Context.Resolve(property.PropertyType));
                    }
                }
            });

            return registration;
        }
    }
}