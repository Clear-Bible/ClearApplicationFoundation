using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Caliburn.Micro;
using ClearApplicationFoundation.Extensions;

namespace ClearApplicationFoundation.Demo;

internal class Bootstrapper : FoundationBootstrapper
{

    protected override void SetupLogging()
    {
        SetupLogging("d:\\temp\\ClearApplication.Demo\\logs\\demo.log");
    }

    protected override void LoadModules(ContainerBuilder builder)
    {
        base.LoadModules(builder);

        builder.RegisterModule<DemoModule>();
    }

  
}