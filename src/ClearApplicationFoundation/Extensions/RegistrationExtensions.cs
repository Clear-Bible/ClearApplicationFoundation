using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
using FluentValidation;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac.Features.Metadata;
using Autofac.Core.Lifetime;
using ClearApplicationFoundation.ViewModels.Infrastructure;

namespace ClearApplicationFoundation.Extensions
{

    public static class RegistrationExtensions
    {
        /// <summary>
        /// Register types that implement <see cref="FluentValidation.IValidator"/> in the provided assemblies.
        /// </summary>
        /// <param name="builder">The container builder.</param>
        /// <param name="assemblies">Assemblies to scan for controllers.</param>
        /// <returns>Registration builder allowing the controller components to be customized.</returns>
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterValidators(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            return builder.RegisterAssemblyTypes(assemblies)
                .Where(t => typeof(IValidator).IsAssignableFrom(t))
                .ExternallyOwned();
        }


        public static void LoadModuleAssemblies(this ContainerBuilder builder, string searchPattern = "*.Module.dll", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var moduleAssemblies = Directory.GetFiles(path, searchPattern, searchOption).Select(Assembly.LoadFrom);

            builder.RegisterAssemblyModules(moduleAssemblies.ToArray());

            //foreach (var assembly in Directory.GetFiles(path, searchPattern, searchOption).Select(Assembly.LoadFrom))
            //{
            //    builder.RegisterAssemblyModules(assembly);
            //}
        }

        public static IEnumerable<Assembly> LoadModuleAssemblies(this IEnumerable<Assembly> selectedAssemblies, string searchPattern = "*.Module.dll", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var assemblies = selectedAssemblies.ToList();
            var moduleAssemblies = Directory.GetFiles(Environment.CurrentDirectory, searchPattern, searchOption)
                .Select(Assembly.LoadFrom);

            assemblies.AddRange(moduleAssemblies);

            return assemblies;
        }


        //public static IEnumerable<TService> ResolveOrdered<TService>(this Autofac.ILifetimeScope lifeTimeScope, string orderingMetadataName)
        //{
        //    var itemsWithMeta = lifeTimeScope.Resolve<IEnumerable<Meta<TService>>>();
        //    var sortedItems = itemsWithMeta
        //        .OrderBy(m =>
        //            Convert.ToInt32(m.Metadata[orderingMetadataName]))
        //        .Select(m => m.Value);

        //    return sortedItems;
        //}

        public static IEnumerable<TService> ResolveOrdered<TService>(this IComponentContext context, string orderingMetadataName = "Order")
        {
            var itemsWithMeta = context.Resolve<IEnumerable<Meta<TService>>>();
            var sortedItems = itemsWithMeta
                .OrderBy(m =>
                    Convert.ToInt32(m.Metadata[orderingMetadataName]))
                .Select(m => m.Value);

            return sortedItems;
        }

        public static IEnumerable<TService> ResolveKeyedOrdered<TService>(this IComponentContext context, string key, string orderingMetadataName = "Order")
        {
            var itemsWithMeta = context.ResolveKeyed<IEnumerable<Meta<TService>>>(key);
            var sortedItems = itemsWithMeta
                .OrderBy(m =>
                    Convert.ToInt32(m.Metadata[orderingMetadataName]))
                .Select(m => m.Value);

            return sortedItems;
        }


    }
}
