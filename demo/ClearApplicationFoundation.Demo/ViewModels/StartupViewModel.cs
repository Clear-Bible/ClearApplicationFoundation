
using Autofac;
using Caliburn.Micro;
using ClearApplicationFoundation.ViewModels.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace ClearApplicationFoundation.Demo.ViewModels
{
    public class StartupViewModel : WorkflowShellViewModel, IStartupDialog
    {
        public bool CanCancel => true /* can always cancel */;
        public async void Cancel()
        {
            await TryCloseAsync(false);
        }

       

        public StartupViewModel(INavigationService? navigationService, ILogger<StartupViewModel>? logger, IMediator? mediator, IEventAggregator? eventAggregator, ILifetimeScope? lifetimeScope) : 
            base(navigationService, logger, eventAggregator, mediator, lifetimeScope)
        {
            CanOk = true;
            Title = "Startup Dialog";
        }

        private bool _canOk;
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

            var views = IoC.GetAll<IWorkflowStepViewModel>();

            foreach (var view in views)
            {
                Steps!.Add(view);
            }

            CurrentStep = Steps![0];
            IsLastWorkflowStep = (Steps.Count == 1);

            EnableControls = true;
            await ActivateItemAsync(Steps[0], cancellationToken);
        }

        public object ExtraData { get; set; }
    }
}
