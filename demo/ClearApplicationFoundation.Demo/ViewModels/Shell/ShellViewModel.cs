using Autofac;
using Caliburn.Micro;
using ClearApplicationFoundation.ViewModels.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Navigation;
using ClearApplicationFoundation.Demo.Views;

namespace ClearApplicationFoundation.Demo.ViewModels.Shell
{
    public class ShellViewModel : ApplicationScreen, IShellViewModel
    {
        public ShellViewModel(INavigationService navigationService, ILogger<ShellViewModel> logger, IEventAggregator eventAggregator, IMediator mediator, ILifetimeScope lifetimeScope) :
            base(navigationService, logger, eventAggregator, mediator, lifetimeScope)
        {
            Title = "Foundation Demo Application";
            LoadingApplication = true;
            NavigationService.Navigated += NavigationServiceOnNavigated;
        }

      

        private bool _loadingApplication;
        public bool LoadingApplication
        {
            get => _loadingApplication;
            set => Set(ref _loadingApplication, value);
        }


        private void NavigationServiceOnNavigated(object sender, NavigationEventArgs e)
        {
            var uri = e.Uri;

            if (uri.OriginalString.Contains(nameof(HomeView)))
            {
                LoadingApplication = false;
            }
        }


        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            NavigationService.Navigated -= NavigationServiceOnNavigated;
            return base.OnDeactivateAsync(close, cancellationToken);
        }

     

    }
}
