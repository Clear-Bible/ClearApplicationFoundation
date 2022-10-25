using Autofac;
using Autofac.Extensions.DependencyInjection;
using Caliburn.Micro;
using ClearApplicationFoundation.Extensions;
using ClearApplicationFoundation.Framework;
using ClearApplicationFoundation.ViewModels;
using ClearApplicationFoundation.ViewModels.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
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
using System.Windows.Threading;
using ClearApplicationFoundation.LogHelpers;

namespace ClearApplicationFoundation
{

    public enum ApplicationExitCodes
    {
        AutofacContainerIsNull = 100,
        MainWindowIsNull = 101,
        EndUserExitedApplication = 102

    }
    public class FoundationBootstrapper : BootstrapperBase
    {
       

        protected IContainer? Container { get; private set; }

        protected ILogger<FoundationBootstrapper>? Logger { get; private set; }

        protected INavigationService? NavigationService { get; private set; }

        protected bool ExitingApplication { get; private set; }

        protected bool DependencyInjectionLogging { get; set; } = false;

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
            Application.Current.Deactivated += ApplicationOnDeactivated;
        }

        protected override async void OnStartup(object sender, StartupEventArgs e)
        {
            await DisplayRootViewForAsync<IShellViewModel>();

            HideMainWindow();

            AddFrameToMainWindow();

            ConfigureNavigationService();

            await NavigateToMainWindow();
        }

        protected virtual void HideMainWindow()
        {
            var mainWindow = Application.Current.MainWindow;
            if (mainWindow == null)
            {
                var exitCode = (int)ApplicationExitCodes.MainWindowIsNull;
                Logger?.LogError($"The application 'main' window is null. Exiting the application with code '{exitCode}' ({ApplicationExitCodes.MainWindowIsNull}");
                Application.Current.Shutdown(exitCode);
            }

            if (mainWindow is { IsVisible: true })
            {
                mainWindow.Hide();
            }
        }

        protected virtual async Task NavigateToMainWindow()
        {
            EnsureApplicationMainWindowVisible();
            RestoreMainWindowState();
            NavigateToViewModel<PlaceHolderMainWindowViewModel>();
            await Task.CompletedTask;
        }

        protected void EnsureApplicationMainWindowVisible()
        {
            var mainWindow = Application.Current.MainWindow;
            if (mainWindow == null)
            {
                var exitCode = (int)ApplicationExitCodes.MainWindowIsNull;
                Logger?.LogError($"The application 'main' window is null. Exiting the application with code '{exitCode}' ({ApplicationExitCodes.MainWindowIsNull}");
                Application.Current.Shutdown(exitCode);
            }

            if (!mainWindow!.IsVisible)
            {
                mainWindow.Show();
            }

            // NB: It is important to change WindowState after the MainWindow has been shown.
            //     Otherwise it will fall behind all of the open apps.
            if (mainWindow.WindowState == WindowState.Minimized)
            {
                mainWindow.WindowState = WindowState.Normal;
            }
        }

        protected virtual async Task ShowStartupDialog<TStartupDialogViewModel, TNavigateToViewModel>(double displayDelayInMilliseconds = 100)
            where TStartupDialogViewModel : IStartupDialog
            where TNavigateToViewModel : notnull
        {

            var mainWindow = Application.Current.MainWindow;
            if (mainWindow == null)
            {
                var exitCode = (int)ApplicationExitCodes.MainWindowIsNull;
                Logger?.LogError($"The application 'main' window is null. Exiting the application with code '{exitCode}' ({ApplicationExitCodes.MainWindowIsNull}");
                Application.Current.Shutdown(exitCode);
            }

            if (mainWindow!.Visibility == Visibility.Visible)
            {
                mainWindow.Hide();
            }


            if (Container == null)
            {
                var exitCode = (int)ApplicationExitCodes.AutofacContainerIsNull;
                Logger?.LogError($"The Autofac 'Container' is null. Exiting the application with code '{exitCode}' ({ApplicationExitCodes.AutofacContainerIsNull}");
                Application.Current.Shutdown(exitCode);
            }

            var windowManager = Container?.Resolve<IWindowManager>();
            var startupViewModel = Container!.Resolve<TStartupDialogViewModel>();


            // Ensure the application will actually close when the main app widow is closed.
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            var result = await windowManager?.ShowDialogAsync(startupViewModel)!;

            if (result.HasValue && result.Value)
            {
                RestoreMainWindowState();
                if (!mainWindow.IsVisible)
                {
                    mainWindow.Show();
                }

                // NB: It is important to change WindowState after the MainWindow has been shown.
                //     Otherwise it will fall behind all of the open apps.
                if (mainWindow.WindowState == WindowState.Minimized)
                {
                    mainWindow.WindowState = WindowState.Normal;
                }

                // wait a short amount of time to allow the ShellView to be rendered.
                await Task.Delay(TimeSpan.FromMilliseconds(displayDelayInMilliseconds));

                // This passes the data to the view model we're navigating to.  The
                // view model should have a property named "Parameter" which can be
                // strongly typed, for example  - public string Parameter { get ;set }
                NavigateToViewModel<TNavigateToViewModel>(startupViewModel.ExtraData);
            }
            else
            {
                var exitCode = (int)ApplicationExitCodes.EndUserExitedApplication;
                Logger?.LogError($"The end user has canceled the startup dialog. Exiting the application with code '{exitCode}' ({ApplicationExitCodes.EndUserExitedApplication}");
                Application.Current.Shutdown(exitCode);
                
            }
        }

        private void ApplicationOnDeactivated(object? sender, EventArgs e)
        {
            SaveMainWindowState();
        }

        protected virtual void RestoreMainWindowState()
        {
            //no-op
        }

        protected virtual void SaveMainWindowState()
        {

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

        protected void NavigateToViewModel<TViewModel>(object? extraData = null)
        {
            NavigationService?.NavigateToViewModel<TViewModel>(extraData);
        }


        protected virtual void PopulateServiceCollection(ServiceCollection serviceCollection)
        {
            //no-op
        }

        protected override void Configure()
        {
            var serviceCollection = new ServiceCollection();
            var builder = new ContainerBuilder();
            PopulateServiceCollection(serviceCollection);
            builder.Populate(serviceCollection);

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

        protected void LogDependencyInjectionRegistrations()
        {
            if (!ExitingApplication && DependencyInjectionLogging)
            {
                var componentRegistrations = Container!.ComponentRegistry.Registrations;

                Logger?.LogDebug("************************************************");
                Logger?.LogDebug("Dependency Injection Registrations");
                foreach (var componentRegistration in componentRegistrations)
                {
                    foreach (var componentRegistrationService in componentRegistration.Services)
                    {
                        Logger?.LogDebug(componentRegistrationService.Description);
                    }

                }

                Logger?.LogDebug("************************************************");
            }
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

            CaptureFilePathHook logFilePathHook = Container!.Resolve<CaptureFilePathHook>();
            var log = new LoggerConfiguration()
                .MinimumLevel.Is(logLevel)
                .WriteTo.File(logPath, outputTemplate: outputTemplate, rollingInterval: RollingInterval.Day, hooks: logFilePathHook)
                .WriteTo.Debug(outputTemplate: outputTemplate)
                .CreateLogger();

            var loggerFactory = Container!.Resolve<ILoggerFactory>();
            loggerFactory.AddSerilog(log);

            Logger = Container!.Resolve<ILogger<FoundationBootstrapper>>();

            Logger.LogDebug($"Application logging has been configured.  Writing logs to '{logPath}'");
        }

        protected override object GetInstance(Type service, string key)
        {
            if (!ExitingApplication && DependencyInjectionLogging)
            {
                Logger!.LogDebug($"GetInstance - fetching '{service.Name}' from DI container.");
            }
           
            return string.IsNullOrEmpty(key) ? Container!.Resolve(service) : Container!.ResolveNamed(key, service);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            var type = typeof(IEnumerable<>).MakeGenericType(service);
            var instances = (Container?.Resolve(type) as IEnumerable<object>)!.ToList();

            if (!ExitingApplication && DependencyInjectionLogging)
            {
                Logger!.LogDebug($"GetAllInstances - Found {instances.Count} of type '{service.FullName}'.");
            }

            if (instances is { Count: > 1 } && service.Name == "IMediator")
            {
                if (DependencyInjectionLogging)
                {
                    Logger!.LogDebug($"Found {instances.Count} instances of IMediator, returning just one.");
                }

                return new List<object>(new [] {instances.First()}!);
            }

            return instances;
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
            Logger?.LogDebug("Adding Frame to ShellView grid control.");

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

        #region Global error handling
        /// <summary>
        /// Handle the system wide exceptions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            Logger?.LogError(e.Exception, "An unhandled error as occurred");
            MessageBox.Show(e.Exception.Message, "An error as occurred", MessageBoxButton.OK);
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            ExitingApplication = true;
            Application.Current.Deactivated -= ApplicationOnDeactivated;
            base.OnExit(sender, e);
        }

        #endregion
    }
}
