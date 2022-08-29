
using Autofac;
using Caliburn.Micro;
using ClearApplicationFoundation.ViewModels.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace ClearApplicationFoundation.Demo.ViewModels
{
    public class StartupViewModel : WorkflowShellViewModel
    {
        public bool CanCancel => true /* can always cancel */;
        public async void Cancel()
        {
            await TryCloseAsync(false);
        }


        private bool _canOk;

        public StartupViewModel(IMediator? mediator, ILogger<StartupViewModel>? logger, INavigationService? navigationService, IEventAggregator? eventAggregator, ILifetimeScope? lifetimeScope) : base(mediator, logger, navigationService, eventAggregator, lifetimeScope)
        {
            CanOk = true;
            DisplayName = "Startup Dialog";
        }

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

            await base.OnInitializeAsync(cancellationToken);

            var views = IoC.GetAll<IWorkflowStepViewModel>();

            foreach (var view in views)
            {
                Steps!.Add(view);
            }

            CurrentStep = Steps![0];
            IsLastWorkflowStep = (Steps.Count == 1);
            await ActivateItemAsync(Steps[0], cancellationToken);
        }

    }
}
