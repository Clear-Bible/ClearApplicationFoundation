using Autofac;
using Caliburn.Micro;
using ClearApplicationFoundation.Extensions;
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
using System.Windows.Controls;

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
         
            var builder = new ContainerBuilder();

            LoadModules(builder);

            Container = builder.Build();

            SetupLogging();

        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            var assemblies =  base.SelectAssemblies().ToList();

            assemblies.Add(Assembly.GetAssembly(typeof(FoundationBootstrapper)));

            return assemblies.LoadModuleAssemblies();
        }

        protected virtual void LoadModules(ContainerBuilder builder)
        {
            builder.RegisterModule<FoundationModule>();
            builder.LoadModuleAssemblies();
        }


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

            Logger.LogDebug($"Application logging has been configured.  Writing logs to '{logPath}'");
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

        /// <summary>
        /// Adds the Frame to the Grid control on the ShellView
        /// </summary>
        /// <param name="frame"></param>
        /// <exception cref="NullReferenceException"></exception>
        private void AddFrameToMainWindow(Frame frame)
        {
            Logger.LogInformation("Adding Frame to ShellView grid control.");

            var mainWindow = Application.MainWindow;
            if (mainWindow == null)
            {
                throw new NullReferenceException("'Application.MainWindow' is null.");
            }


            if (mainWindow.Content is not Grid grid)
            {
                throw new NullReferenceException("The grid on 'Application.MainWindow' is null.");
            }

            Grid.SetRow(frame, 1);
            Grid.SetColumn(frame, 0);
            Panel.SetZIndex(frame, 0);
            grid.Children.Add(frame);
        }
    }
}
