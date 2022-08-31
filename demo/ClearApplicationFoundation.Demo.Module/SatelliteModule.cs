using Autofac;
using ClearApplicationFoundation.Demo.Module.ViewModels;
using ClearApplicationFoundation.Framework;

namespace ClearApplicationFoundation.Demo.Module
{
    internal class SatelliteModule : Autofac.Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Tab1ViewModel>().As<ITab>().InstancePerDependency();
            builder.RegisterType<Tab2ViewModel>().As<ITab>().InstancePerDependency();
        }
    }
}
