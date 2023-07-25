using ClearApplicationFoundation.ViewModels.Infrastructure;
using System.Threading.Tasks;
using System.Threading;
using ClearApplicationFoundation.Services;
using Microsoft.Extensions.Logging;

namespace ClearApplicationFoundation.Demo.ViewModels.Help;

public class StartupHelpCompleteViewModel : HelpOverlayStepViewModel<StartupHelpViewModel>
{

    public StartupHelpCompleteViewModel()
    {
        
    }

    protected StartupHelpCompleteViewModel(ILogger<StartupHelpCompleteViewModel> logger, ILocalizationService localizationService)
        : base(logger, localizationService)
    {

    }

    protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
    {

        CanMoveForwards = true;
        CanMoveBackwards = true;
        EnableControls = true;

        await Task.CompletedTask;
    }

    public void Complete()
    {
        ParentViewModel!.IsVisible = false;
    }
}