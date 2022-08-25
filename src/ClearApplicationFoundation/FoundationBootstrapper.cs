using Autofac;
using Caliburn.Micro;
using ClearApplicationFoundation.ViewModels;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace ClearApplicationFoundation
{
    public class FoundationBootstrapper : BootstrapperBase
    {
        protected IContainer? Container { get; private set; }

        protected ILogger<FoundationBootstrapper>? Logger { get; private set; }

        public FoundationBootstrapper()
        {
            Initialize();
        }

        protected override async void OnStartup(object sender, StartupEventArgs e)
        {
            await DisplayRootViewForAsync<ShellViewModel>();
        }


        protected override void Configure()
        {
            AssemblySource.Instance.AddRange(PublishSingleFileBypassAssemblies);


            var builder = new ContainerBuilder();

            LoadModules(builder);

            Container = builder.Build();

            SetupLogging();

           
        }

        protected virtual void LoadModules(ContainerBuilder builder)
        {
            builder.RegisterModule<FoundationModule>();
        }


        /// <summary>
        /// When your application is deployed using PublishSingleFile under .NET5+, override
        /// this to explicitly list the assemblies that MEF needs to search exports for.
        /// </summary>
        protected virtual IEnumerable<Assembly?> PublishSingleFileBypassAssemblies => Enumerable.Empty<Assembly>();

        protected virtual void SetupLogging()
        {
            SetupLogging(Path.Combine(Environment.CurrentDirectory, "Logs\\foundation.log"));
        }


        // ReSharper disable once RedundantAssignment
        protected void SetupLogging(string logPath, LogEventLevel logLevel = LogEventLevel.Information, string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] [{SourceContext}] {Message}{NewLine}{Exception}")
        {
            
#if DEBUG
            logLevel = LogEventLevel.Verbose;
#endif
          
            var log = new LoggerConfiguration()
                .MinimumLevel.Is(logLevel)
                .WriteTo.File(logPath, outputTemplate: outputTemplate, rollingInterval: RollingInterval.Day)
                .WriteTo.Debug(outputTemplate: outputTemplate)
                .CreateLogger();

            var loggerFactory = Container!.Resolve<ILoggerFactory>();
            loggerFactory.AddSerilog(log);

            Logger = Container!.Resolve<ILogger<FoundationBootstrapper>>();

            Logger.LogDebug("I'm alive!");
        }

        protected override object GetInstance(Type service, string key)
        {
            return string.IsNullOrEmpty(key) ? Container!.Resolve(service) : Container!.ResolveNamed(key, service);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            var type = typeof(IEnumerable<>).MakeGenericType(service);

            return (Container?.Resolve(type) as IEnumerable<object>)!;
        }

        protected override void BuildUp(object instance)
        {
            Container?.InjectProperties(instance);
        }
    }
}
