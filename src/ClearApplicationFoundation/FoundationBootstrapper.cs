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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ClearApplicationFoundation.Framework;
using System.Globalization;
using System.Threading;
using ClearApplicationFoundation.ViewModels.Shell;

namespace ClearApplicationFoundation
{
    public class FoundationBootstrapper : BootstrapperBase
    {
        protected IContainer? Container { get; private set; }

        protected ILogger<FoundationBootstrapper>? Logger { get; private set; }

        public FoundationBootstrapper()
        {
            // ReSharper disable VirtualMemberCallInConstructor
            PreInitialize();
            Initialize();
            PostInitialize();
            // ReSharper enable VirtualMemberCallInConstructor
        }

        protected virtual void PostInitialize()
        {
            //no-op
        }

        protected virtual void PreInitialize()
        {
            //var code = Properties.Settings.Default.LanguageCode;

            //if (!string.IsNullOrWhiteSpace(code))
            //{
            //    var culture = CultureInfo.GetCultureInfo(code);
            //    Thread.CurrentThread.CurrentUICulture = culture;
            //    Thread.CurrentThread.CurrentCulture = culture;
            //}
        }

        protected INavigationService? NavigationService { get; private set; }
        protected override async void OnStartup(object sender, StartupEventArgs e)
        {
            await DisplayRootViewForAsync<IShellViewModel>();

            Application.Current.MainWindow?.Hide();
            AddFrameToMainWindow();

            ConfigureNavigationService();

            await NavigateToMainWindow();
        }

        protected virtual async Task NavigateToMainWindow()
        {

            var mainWindow = Application.Current.MainWindow;
            if (mainWindow == null)
            {
                Application.Current.Shutdown(-101);
            }

            if (!mainWindow!.IsVisible)
            {
                mainWindow.Show();
            }

            // NB: It is important to change WindowState after the MainWindow has been shown.
            //     Otherwise it will fall be hind all of the open apps.
            if (mainWindow.WindowState == WindowState.Minimized)
            {
                mainWindow.WindowState = WindowState.Normal;
            }
            NavigateToViewModel<PlaceHolderMainViewModel>();
            await Task.CompletedTask;
        }

        protected virtual async Task ShowStartupDialog<TStartupDialogViewModel, TNavigateToViewModel>()
            where TStartupDialogViewModel : notnull
            where TNavigateToViewModel : notnull
        {

            var mainWindow = Application.Current.MainWindow;
            if (mainWindow == null)
            {
                Application.Current.Shutdown(-101);
            }

            if (mainWindow!.Visibility == Visibility.Visible)
            {
                mainWindow.Hide();
            }

            if (Container == null)
            {
                Application.Current.Shutdown(-100);
            }

            var windowManager = Container?.Resolve<IWindowManager>();
            var startupViewModel = Container!.Resolve<TStartupDialogViewModel>();


            // Ensure the application will actually close when the main app widow is closed.
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            var result = await windowManager?.ShowDialogAsync(startupViewModel)!;

            if (result.HasValue && result.Value)
            {

                if (!mainWindow.IsVisible)
                {
                    mainWindow.Show();
                }

                // NB: It is important to change WindowState after the MainWindow has been shown.
                //     Otherwise it will fall be hind all of the open apps.
                if (mainWindow.WindowState == WindowState.Minimized)
                {
                    mainWindow.WindowState = WindowState.Normal;
                }

                NavigateToViewModel<TNavigateToViewModel>();
            }
            else
            {
                Application.Current.Shutdown(-102);
            }
        }

        private void ConfigureNavigationService()
        {
            NavigationService = Container?.Resolve<INavigationService>();

            if (NavigationService == null)
            {
                throw new NullReferenceException(
                    "'NavigationService' is null. Please ensure the 'FoundationModule' has been loaded by Autofac.");
            }
        }

        protected void NavigateToViewModel<TViewModel>()
        {
            NavigationService?.NavigateToViewModel<TViewModel>();
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
            var assemblies = base.SelectAssemblies().ToList();

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
        /// <exception cref="NullReferenceException"></exception>
        private void AddFrameToMainWindow()
        {
            Logger?.LogInformation("Adding Frame to ShellView grid control.");

            var frameSet = Container?.Resolve<FrameSet>();

            if (frameSet == null)
            {
                throw new NullReferenceException("'FrameSet' is null. Please ensure the 'FoundationModule' has been load by Autofac.");
            }

            var frame = frameSet.Frame;


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
