using Autofac;
using ClearApplicationFoundation.Demo.ViewModels;
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
        // Show the StatupViewModel as a dialog, then nanivate to HomeViewModel
        // if the dialog result is "true"
        await ShowStartupDialog<StartupViewModel, HomeViewModel>();
    }

}