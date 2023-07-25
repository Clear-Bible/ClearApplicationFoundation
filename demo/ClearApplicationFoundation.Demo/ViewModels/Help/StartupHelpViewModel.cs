
using System.Linq;
using Autofac.Core.Lifetime;
using Caliburn.Micro;
using ClearApplicationFoundation.Exceptions;
using ClearApplicationFoundation.ViewModels.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using Autofac;
using ClearApplicationFoundation.Extensions;

namespace ClearApplicationFoundation.Demo.ViewModels.Help
{
    public class StartupHelpViewModel : HelpOverlayShellViewModel
    {
        public StartupHelpViewModel()
        {

        }

        public StartupHelpViewModel(ILogger<StartupHelpViewModel>? logger, ILifetimeScope? lifetimeScope) : base(logger, lifetimeScope)
        {
            
        }

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            var views = LifetimeScope?.ResolveKeyedOrdered<IWorkflowStepViewModel>("StartupHelp", "Order").ToArray();

            if (views == null || !views.Any())
            {
                throw new DependencyRegistrationMissingException(
                    "There are no dependency injection registrations of 'IWorkflowStepViewModel' with the key of 'StartupHelp'.  Please check the dependency registration in your bootstrapper implementation.");
            }

            foreach (var view in views)
            {
                Steps!.Add(view);
            }

            CurrentStep = Steps[0];

            IsLastWorkflowStep = (Steps.Count == 1);

            EnableControls = true;
            await ActivateItemAsync(Steps[0], cancellationToken);

        }
    }
}
