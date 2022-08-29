using Autofac;
using ClearApplicationFoundation.Demo.ViewModels;
using ClearApplicationFoundation.ViewModels.Shell;
using System.IO;
using System.Threading.Tasks;

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
        SetApplicationName("Foundation Demo App");
        base.PostInitialize();
       
    }

    protected override void SetApplicationName(string applicationName = "Application Foundation Demo")
    {
        var shellViewModel = Container?.Resolve<ShellViewModel>();

        if (shellViewModel != null)
        {
            shellViewModel.SetDisplayName(applicationName);
        }

    }
}