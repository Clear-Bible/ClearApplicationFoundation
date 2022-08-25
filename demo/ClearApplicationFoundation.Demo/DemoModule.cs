using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using ClearApplicationFoundation.Demo.ViewModels;
using ClearApplicationFoundation.Extensions;
using ClearApplicationFoundation.Framework;
using Module = Autofac.Module;
using MediatR.Extensions.Autofac.DependencyInjection;

namespace ClearApplicationFoundation.Demo
{
    internal class DemoModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
                .Where(type => type.Name.EndsWith("ViewModel"))
                .AsSelf()
                .InstancePerDependency();

            //  register views
            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
                .Where(type => type.Name.EndsWith("View"))
                .AsSelf()
                .InstancePerDependency();

            builder.RegisterType<HomeViewModel>().As<IMainWindow>().InstancePerDependency();

            builder.RegisterValidators(Assembly.GetExecutingAssembly());

            builder.RegisterMediatR(typeof(App).Assembly);
        }
    }
}
