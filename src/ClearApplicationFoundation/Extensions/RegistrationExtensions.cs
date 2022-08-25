using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
using FluentValidation;
using System.Reflection;

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
    }
}
