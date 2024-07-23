using Autofac;
using Caliburn.Micro;
using ClearApplicationFoundation.Demo.ViewModels;
using ClearApplicationFoundation.Demo.Views;
using ClearApplicationFoundation.Extensions;
using ClearApplicationFoundation.Framework;
using ClearApplicationFoundation.ViewModels.Infrastructure;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
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
            builder.RegisterType<ShellViewModel>().As<IShellViewModel>().SingleInstance();

            builder.RegisterType<HomeViewModel>().As<IMainWindowViewModel>().SingleInstance();

            // Register validators from this assembly.
            builder.RegisterValidators(Assembly.GetExecutingAssembly());

			// Register Mediator requests and handlers.
			var configuration = MediatRConfigurationBuilder
				.Create(Assembly.GetExecutingAssembly())
				.WithAllOpenGenericHandlerTypesRegistered()
				.WithRegistrationScope(RegistrationScope.Scoped)
				.Build();
			builder.RegisterMediatR(configuration);
			//builder.RegisterMediatR(typeof(App).Assembly);

            //builder.RegisterType<ProjectPickerView>().AsSelf();

            //builder.RegisterType<ProjectSetupView>().AsSelf();

            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
              .Where(type => type.Name.EndsWith("ViewModel"))
              .AsSelf()
              .InstancePerDependency();

            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
                .Where(type => type.Name.EndsWith("View"))
                .AsSelf()
                .InstancePerDependency();

            builder.RegisterType<ProjectPickerViewModel>().As<IWorkflowStepViewModel>();

            builder.RegisterType<ProjectSetupViewModel>().As<IWorkflowStepViewModel>();
        }
    }
}
