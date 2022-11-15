using Autofac;
using Caliburn.Micro;
using MediatR.Extensions.Autofac.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;
using System.Linq;
using ClearApplicationFoundation.Framework;
using ClearApplicationFoundation.LogHelpers;
using ClearApplicationFoundation.ViewModels;
using ClearApplicationFoundation.ViewModels.Shell;
using Microsoft.Extensions.DependencyInjection;
using ClearApplicationFoundation.ViewModels.Infrastructure;

namespace ClearApplicationFoundation
{
    internal class FoundationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            //  register view models
            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
                .Where(type => type.Name.EndsWith("ViewModel"))
                .AsSelf()
                .InstancePerDependency();

            //  register views
            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
                .Where(type => type.Name.EndsWith("View"))
                .AsSelf()
                .InstancePerDependency();

            builder.RegisterType<WindowManager>().As<IWindowManager>().InstancePerLifetimeScope();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().InstancePerLifetimeScope();

            builder.RegisterMediatR(typeof(App).Assembly);

            var frameSet = new FrameSet();
            builder.RegisterInstance(frameSet);
            builder.RegisterInstance(frameSet.NavigationService).As<INavigationService>();

            builder.RegisterType<ShellViewModel>().As<IShellViewModel>();
            builder.RegisterType<PlaceHolderMainWindowViewModel>().As<IMainWindowViewModel>();

            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new SerilogLoggerProvider());
            builder.RegisterInstance(loggerFactory).As<ILoggerFactory>().SingleInstance();
            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).SingleInstance();

            builder.RegisterType<CaptureFilePathHook>().AsSelf().SingleInstance();
        }
    }


}
