using Autofac;
using Caliburn.Micro;
using ClearApplicationFoundation.Demo.ViewModels;
using ClearApplicationFoundation.Extensions;
using ClearApplicationFoundation.Framework;
using ClearApplicationFoundation.ViewModels.Shell;
using MediatR.Extensions.Autofac.DependencyInjection;
using System.Linq;
using System.Reflection;
using Module = Autofac.Module;
using ShellViewModel = ClearApplicationFoundation.Demo.ViewModels.Shell.ShellViewModel;

namespace ClearApplicationFoundation.Demo
{
    internal class DemoModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            // IMPORTANT!  - override the default ShellViewModel from the foundation.
            builder.RegisterType<ShellViewModel>().As<IShellViewModel>();

            // Register validators from this assembly.
            builder.RegisterValidators(Assembly.GetExecutingAssembly());

            // Register Mediator requests and handlers.
            builder.RegisterMediatR(typeof(App).Assembly);
        }
    }
}
