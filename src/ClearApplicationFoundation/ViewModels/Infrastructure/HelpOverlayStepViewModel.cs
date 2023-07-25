using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using ClearApplicationFoundation.Services;
using Microsoft.Extensions.Logging;

namespace ClearApplicationFoundation.ViewModels.Infrastructure;

public abstract class HelpOverlayStepViewModel : Screen, IWorkflowStepViewModel
{

    private bool _isBusy;
    public virtual bool IsBusy
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

    protected ILogger? Logger { get; }

    protected ILocalizationService? LocalizationService { get;  }

    protected HelpOverlayStepViewModel()
    {
        // allows view models to be used in design-time mode.
    }

    protected HelpOverlayStepViewModel(ILogger? logger, ILocalizationService? localizationService)
    {

        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        LocalizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService)); ;
    }

    private Direction _direction;
    public Direction Direction
    {
        get => _direction;
        set => Set(ref _direction, value);
    }

    protected override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        ShowWorkflowButtons();
        EnableControls = ((Parent as HelpOverlayShellViewModel)!).EnableControls;
        return base.OnActivateAsync(cancellationToken);
    }

    private bool _enableControls;
    public bool EnableControls
    {
        get => _enableControls;
        set
        {
            Logger?.LogInformation($"HelpOverlayStepViewModel - Setting EnableControls to {value} at {DateTime.Now:HH:mm:ss.fff}");
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


public abstract class HelpOverlayStepViewModel<TParentViewModel> : HelpOverlayStepViewModel
    where TParentViewModel : class
{

    protected ILocalizationService? LocalizationService { get; }
    public TParentViewModel? ParentViewModel => Parent as TParentViewModel;

    protected HelpOverlayStepViewModel()
    {
        //no op
    }

    protected HelpOverlayStepViewModel(ILogger logger, ILocalizationService localizationService)
        : base(logger, localizationService)
    {

    }

}