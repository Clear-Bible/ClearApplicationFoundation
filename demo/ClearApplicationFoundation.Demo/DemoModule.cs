using Autofac;
using Caliburn.Micro;
using ClearApplicationFoundation.Demo.ViewModels;
using ClearApplicationFoundation.Demo.Views;
using ClearApplicationFoundation.Extensions;
using ClearApplicationFoundation.Framework;
using ClearApplicationFoundation.ViewModels.Infrastructure;
using MediatR.Extensions.Autofac.DependencyInjection;
using System.Linq;
using System.Reflection;
using ClearApplicationFoundation.Demo.Services;
using ClearApplicationFoundation.Demo.ViewModels.Help;
using ClearApplicationFoundation.Services;
using Module = Autofac.Module;
using ShellViewModel = ClearApplicationFoundation.Demo.ViewModels.Shell.ShellViewModel;

namespace ClearApplicationFoundation.Demo
{

    internal static class ContainerBuilderExtensions
    {
        public static void RegisterStartupDialogHelpDependencies(this ContainerBuilder builder)
        {


            builder.RegisterType<StartupHelpViewModel>().AsSelf();
            builder.RegisterType<StartupHelpWelcomeViewModel>().As<IWorkflowStepViewModel>()
                .Keyed<IWorkflowStepViewModel>("StartupHelp")
                .WithMetadata("Order", 1);

            builder.RegisterType<StartupHelpCompleteViewModel>().As<IWorkflowStepViewModel>()
                .Keyed<IWorkflowStepViewModel>("StartupHelp")
                .WithMetadata("Order", 2);
        }

        public static void RegisterStartupDialogDependencies(this ContainerBuilder builder)
        {

            builder.RegisterType<ProjectPickerViewModel>().As<IWorkflowStepViewModel>()
                .Keyed<IWorkflowStepViewModel>("Startup")
                .WithMetadata("Order", 1);

            builder.RegisterType<ProjectSetupViewModel>().As<IWorkflowStepViewModel>()
               .Keyed<IWorkflowStepViewModel>("Startup")
                .WithMetadata("Order", 2);
        }
    }

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
            builder.RegisterMediatR(typeof(App).Assembly);

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

            builder.RegisterType<LocalizationService>().As<ILocalizationService>().InstancePerDependency();

           
            builder.RegisterStartupDialogDependencies();
            builder.RegisterStartupDialogHelpDependencies();
        }
    }
}
