using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Caliburn.Micro;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClearApplicationFoundation.ViewModels.Infrastructure;

public abstract class WorkflowStepViewModel : ApplicationScreen, IWorkflowStepViewModel
{

    private Direction _direction;
    public Direction Direction
    {
        get => _direction;
        set => Set(ref _direction, value);
    }

    //INavigationService? navigationService, ILogger? logger, IEventAggregator? eventAggregator, IMediator? mediator, ILifetimeScope? lifetimeScope
    protected WorkflowStepViewModel(INavigationService navigationService, ILogger logger, IEventAggregator eventAggregator,IMediator mediator, ILifetimeScope? lifetimeScope) :
        base(navigationService, logger, eventAggregator, mediator, lifetimeScope)
    {

    }

    protected override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        ShowWorkflowButtons();
        EnableControls = ((Parent as WorkflowShellViewModel)!).EnableControls;
        return base.OnActivateAsync(cancellationToken);
    }

    private bool _enableControls;
    public bool EnableControls
    {
        get => _enableControls;
        set
        {
            Logger!.LogInformation($"WorkflowStepViewModel - Setting EnableControls to {value} at {DateTime.Now:HH:mm:ss.fff}");
            //(Parent as WorkflowShellViewModel).EnableControls = value;
            Set(ref _enableControls, value);
        }
    }


    private bool _canMoveForwards;
    private bool _canMoveBackwards;

    public bool CanMoveForwards
    {
        get => _canMoveForwards;
        set => Set(ref _canMoveForwards, value);
    }

    public bool CanMoveBackwards
    {
        get => _canMoveBackwards;
        set => Set(ref _canMoveBackwards, value);
    }

    public virtual async Task MoveForwardsAction()
    {
        await Task.CompletedTask;
    }

    public virtual async Task MoveBackwardsAction()
    {
        await Task.CompletedTask;
    }


    public async Task MoveForwards()
    {
        Direction = Direction.Forwards;
        await MoveForwardsAction();
        await TryCloseAsync();
    }

    public async Task MoveBackwards()
    {
        Direction = Direction.Backwards;
        await MoveBackwardsAction();
        await TryCloseAsync();
    }

    private bool _showBackButton;
    public bool ShowBackButton
    {
        get => _showBackButton;
        set => Set(ref _showBackButton, value);
    }


    private bool _showForwardButton;
    public bool ShowForwardButton
    {
        get => _showForwardButton;
        set => Set(ref _showForwardButton, value);
    }

    protected void ShowWorkflowButtons()
    {
        var steps = (Parent as WorkflowShellViewModel)?.Steps;
        if (steps != null)
        {
            var index = steps.IndexOf(this);

            if (index == 0 && steps.Count == 1)
            {

                ShowBackButton = false;
                ShowForwardButton = false;
            }

            if (index > 0 && index < steps.Count - 1)
            {

                ShowBackButton = true;
                ShowForwardButton = true;
            }


            if (index == 0 && steps.Count > 1)
            {

                ShowBackButton = false;
                ShowForwardButton = true;
            }

            if (steps.Count > 1 && index == steps.Count - 1)
            {
                ShowBackButton = true;
                ShowForwardButton = false;
            }
        }
    }
}