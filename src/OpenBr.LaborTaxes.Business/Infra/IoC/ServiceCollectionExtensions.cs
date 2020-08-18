using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OpenBr.LaborTaxes.Business.Infra.IoC
{

    /// <summary>
    /// Dependency Injection Services Collection Extension
    /// </summary>
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Records all types
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="services">Service collection</param>
        /// <param name="lifetime">Life time</param>
        /// <param name="assemblies">Assemblies list</param>
        public static void RegisterAllTypes<T>(this IServiceCollection services, ServiceLifetime lifetime, params Assembly[] assemblies)
        {
            IEnumerable<TypeInfo> tipos = assemblies.SelectMany(a => a.DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(T))));
            foreach (TypeInfo type in tipos)
            {
                services.Add(new ServiceDescriptor(typeof(T), type, lifetime));
            }
        }

    }

}
