using Autofac;
using ClearApplicationFoundation.Demo.ViewModels;
using ClearApplicationFoundation.ViewModels.Shell;
using System.IO;
using System.Threading.Tasks;
using ClearApplicationFoundation.Demo.DependencyInjectionTest;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShellViewModel = ClearApplicationFoundation.Demo.ViewModels.Shell.ShellViewModel;

namespace ClearApplicationFoundation.Demo;

internal class Bootstrapper : FoundationBootstrapper
{

    protected override void SetupLogging()
    {
        SetupLogging(Path.Combine(Path.GetTempPath(),"ClearApplication.Demo\\logs\\demo.log"));
    }

    protected override void LoadModules(ContainerBuilder builder)
    {
        base.LoadModules(builder);
        builder.RegisterModule<DemoModule>();
    }

    protected override async Task NavigateToMainWindow()
    {
        // Show the StartupViewModel as a dialog, then navigate to HomeViewModel
        // if the dialog result is "true"
        await ShowStartupDialog<StartupViewModel, HomeViewModel>();
    }

    protected override void PostInitialize()
    {
        Logger?.LogInformation("PostInitialize called.");
        base.PostInitialize();
    }

    protected override void PopulateServiceCollection(ServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<TestService>();
        base.PopulateServiceCollection(serviceCollection);
    }

    protected override void RestoreMainWindowState()
    {
        Logger?.LogInformation("RestoreMainWindowState called.");
        base.RestoreMainWindowState();
    }

    protected override void SaveMainWindowState()
    {
        Logger?.LogInformation("SaveMainWindowState called.");
        base.SaveMainWindowState();
    }
}