using Autofac;
using Caliburn.Micro;
using ClearApplicationFoundation.ViewModels.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClearApplicationFoundation.Demo.ViewModels.Shell
{
    public class ShellViewModel : ApplicationScreen, IShellViewModel
    {
        public ShellViewModel(INavigationService navigationService, ILogger<ShellViewModel> logger, IEventAggregator eventAggregator, IMediator mediator, ILifetimeScope lifetimeScope) :
            base(navigationService, logger, eventAggregator, mediator, lifetimeScope)
        {
            Title = "Foundation Demo Application";
        }

    }
}
