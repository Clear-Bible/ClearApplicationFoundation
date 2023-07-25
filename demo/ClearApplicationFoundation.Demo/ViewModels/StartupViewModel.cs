
using System.Linq;
using Autofac;
using Caliburn.Micro;
using ClearApplicationFoundation.LogHelpers;
using ClearApplicationFoundation.ViewModels.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using ClearApplicationFoundation.Demo.ViewModels.Help;
using ClearApplicationFoundation.Extensions;

namespace ClearApplicationFoundation.Demo.ViewModels
{
    public class StartupViewModel : WorkflowShellViewModel, IStartupDialog
    {
        public bool CanCancel => true /* can always cancel */;
        public async void Cancel()
        {
            await TryCloseAsync(false);
        }


        public StartupHelpViewModel StartupHelpViewModel
        {
            get => startupHelpViewModel_;
            set => Set(ref startupHelpViewModel_, value);
        }


        public StartupViewModel(StartupHelpViewModel startupHelpViewModel, INavigationService? navigationService, ILogger<StartupViewModel>? logger, IMediator? mediator, IEventAggregator? eventAggregator, ILifetimeScope? lifetimeScope) : 
            base(navigationService, logger, eventAggregator, mediator, lifetimeScope)
        {
            CanOk = true;
            Title = "Startup Dialog";
            StartupHelpViewModel = startupHelpViewModel;

            StartupHelpViewModel.IsVisible = true;
         
            
            var logFilePath = IoC.Get<CaptureFilePathHook>();
        }

        private bool _canOk;
        private StartupHelpViewModel startupHelpViewModel_;

        public bool CanOk
        {
            get => _canOk;
            set => Set(ref _canOk, value);
        }

        public async void Ok()
        {
            await TryCloseAsync(true);
        }

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            ExtraData = "flow";
            await base.OnInitializeAsync(cancellationToken);

            var views = LifetimeScope?.ResolveKeyedOrdered<IWorkflowStepViewModel>("Startup", "Order").ToArray();

            foreach (var view in views)
            {
                Steps!.Add(view);
            }

            CurrentStep = Steps![0];
            IsLastWorkflowStep = (Steps.Count == 1);

            EnableControls = true;
            await ActivateItemAsync(Steps[0], cancellationToken);

            await ScreenExtensions.TryActivateAsync(StartupHelpViewModel, cancellationToken);

        }

        public object ExtraData { get; set; }

        public void ShowHelp()
        {
            StartupHelpViewModel.IsVisible = true;
        }
    }
}
