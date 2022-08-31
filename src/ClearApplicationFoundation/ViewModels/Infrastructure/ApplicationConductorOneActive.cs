using Caliburn.Micro;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using System;
using Autofac;

namespace ClearApplicationFoundation.ViewModels.Infrastructure;

public abstract class ApplicationConductorOneActive<T> : Conductor<T>.Collection.OneActive where T : class
{
    protected INavigationService? NavigationService { get; }
    protected IEventAggregator? EventAggregator { get; }
    protected IMediator? Mediator { get; }
    protected ILogger Logger { get; }
    protected ILifetimeScope? LifetimeScope { get; set; }   

    protected ApplicationConductorOneActive(INavigationService? navigationService, ILogger? logger, IEventAggregator? eventAggregator, IMediator? mediator, ILifetimeScope? lifetimeScope)
    {
        NavigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        EventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
        Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        LifetimeScope = lifetimeScope?? throw new ArgumentNullException(nameof(lifetimeScope));
    }

    private string? _title;
    public string? Title
    {
        get => _title;
        set => Set(ref _title, value);
    }

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set => Set(ref _isBusy, value, nameof(IsBusy));
    }

    protected override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        Logger?.LogInformation($"Subscribing {this.GetType().Name} to the EventAggregator");
        EventAggregator.SubscribeOnUIThread(this);
        return base.OnActivateAsync(cancellationToken);
    }

    protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
    {
        Logger?.LogInformation($"Unsubscribing {this.GetType().Name} from the EventAggregator");
        EventAggregator?.Unsubscribe(this);
        return base.OnDeactivateAsync(close, cancellationToken);
    }

    protected async Task SendProgressBarMessage(string message, double delayInSeconds = 1.0)
    {
        Logger?.LogInformation(message);
        await Task.Delay(TimeSpan.FromSeconds(delayInSeconds));
        await EventAggregator.PublishOnUIThreadAsync(new ProgressBarMessage(message));
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
            return Mediator?.Send(request, cancellationToken)!;
        }
        finally
        {
            IsBusy = false;
        }
    }

}