using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClearApplicationFoundation.ViewModels.Infrastructure
{

    public record ProgressBarVisibilityMessage(bool Show);
    public record ProgressBarMessage(string Message);

    public abstract class ApplicationScreen : Screen, IDisposable
    {
        protected ILogger? Logger { get; private set; }
        protected INavigationService? NavigationService { get; private set; }
        protected IEventAggregator? EventAggregator { get; private set; }
        protected IMediator? Mediator { get; private set; }
        
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => Set(ref _isBusy, value, nameof(IsBusy));
        }

        private FlowDirection _windowFlowDirection = FlowDirection.LeftToRight;
        public FlowDirection WindowFlowDirection
        {
            get => _windowFlowDirection;
            set => Set(ref _windowFlowDirection, value, nameof(WindowFlowDirection));
        }

        private string? _title;
        public string? Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        protected ApplicationScreen()
        {

        }

        protected ApplicationScreen(INavigationService? navigationService, ILogger? logger, IEventAggregator? eventAggregator, IMediator? mediator)
        {
            NavigationService = navigationService;
            Logger = logger;
            EventAggregator = eventAggregator;
            Mediator = mediator;
        }

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation($"Subscribing {this.GetType().Name} to the EventAggregator");
            EventAggregator.SubscribeOnUIThread(this);
            return base.OnActivateAsync(cancellationToken);
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            Logger.LogInformation($"Unsubscribing {this.GetType().Name} from the EventAggregator");
            EventAggregator.Unsubscribe(this);
            return base.OnDeactivateAsync(close, cancellationToken);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose of unmanaged resources here...
            }
        }



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected async Task SendProgressBarMessage(string message, double delayInSeconds = 1.0)
        {
            Logger.LogInformation(message);
            await Task.Delay(TimeSpan.FromSeconds(delayInSeconds));
            await EventAggregator.PublishOnUIThreadAsync(
                new ProgressBarMessage(message));
        }

        protected async Task SendProgressBarVisibilityMessage(bool show, double delayInMilliseconds = 0.0)
        {
            if (delayInMilliseconds > 0.0)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(delayInMilliseconds));
            }
            await EventAggregator.PublishOnUIThreadAsync(new ProgressBarVisibilityMessage(show));
        }

        public virtual Task<TResponse> ExecuteRequest<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
        {
            IsBusy = true;
            try
            {
                return Mediator.Send(request, cancellationToken);
            }
            finally
            {
                IsBusy = false;
            }
        }


    }
}
