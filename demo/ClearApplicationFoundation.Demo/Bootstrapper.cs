using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using Autofac.Core;
using Caliburn.Micro;
using ClearApplicationFoundation.Demo.ViewModels;
using ClearApplicationFoundation.Extensions;

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

        await ShowStartupDialog<StartupViewModel, HomeViewModel>();
    }

  


    //    Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
    //    CustomDialog dialog = new CustomDialog();
    //    bool? dialogResult = auth.ShowDialog();
    //        /* Handle results */
    //        if (dialogResult.HasValue && dialogResult.Value)
    //    {
    //        base.OnStartup(e);
    //    }
    //else
    //{
    //    this.Shutdown();
    //}


}