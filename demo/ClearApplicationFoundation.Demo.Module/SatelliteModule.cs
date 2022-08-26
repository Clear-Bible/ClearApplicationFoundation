using Autofac;
using Caliburn.Micro;
using System.Linq;
using ClearApplicationFoundation.Demo.Module.ViewModels;
using ClearApplicationFoundation.Framework;
using Microsoft.Extensions.Logging;

namespace ClearApplicationFoundation.Demo.Module
{
    internal class SatelliteModule : Autofac.Module
    {
        private readonly ILogger<SatelliteModule> _logger;

        //public SatelliteModule(ILogger<SatelliteModule> logger)
        //{
        //    _logger = logger;
        //}
        protected override void Load(ContainerBuilder builder)
        {
            // _logger.LogInformation($"Loading '{nameof(SatelliteModule)}' module");

            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
                .Where(type => type.Name.EndsWith("ViewModel"))
                .AsSelf()
                .InstancePerDependency();


            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
                .Where(type => type.Name.EndsWith("View"))
                .AsSelf()
                .InstancePerDependency();

            builder.RegisterType<Tab1ViewModel>().As<ITab>().InstancePerDependency();
            builder.RegisterType<Tab2ViewModel>().As<ITab>().InstancePerDependency();


        }
    }
}
