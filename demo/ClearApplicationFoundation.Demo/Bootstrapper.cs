using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Autofac;
using Caliburn.Micro;

namespace ClearApplicationFoundation.Demo
{
    internal class Bootstrapper : FoundationBootstrapper
    {

        protected override IEnumerable<Assembly?> PublishSingleFileBypassAssemblies
        {
            get { yield return Assembly.GetAssembly(typeof(FoundationBootstrapper)); }
        }

        protected override void SetupLogging()
        {
            SetupLogging(Path.Combine(Environment.CurrentDirectory, "Logs\\demo.log"));
        }

        protected override void LoadModules(ContainerBuilder builder)
        {
            base.LoadModules(builder);

            builder.RegisterModule<DemoModule>();
        }
    }
}
