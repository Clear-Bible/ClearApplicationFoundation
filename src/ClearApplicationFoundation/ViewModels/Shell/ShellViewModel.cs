using System.Threading;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Caliburn.Micro;
using ClearApplicationFoundation.ViewModels.Infrastructure;

namespace ClearApplicationFoundation.ViewModels.Shell
{
    public class ShellViewModel : Screen, IShellViewModel
    {
        private readonly INavigationService _navigationService;
        private string? _title;
        private bool _loadingApplication;

        public ShellViewModel()
        {
            Title = "Clear Application Foundation";
            LoadingApplication = true;
        }

        public bool LoadingApplication
        {
            get => _loadingApplication;
            set => Set(ref _loadingApplication, value);
        }

        public ShellViewModel(INavigationService navigationService)
        {
            Title = "Clear Application Foundation";
            LoadingApplication = true;
            _navigationService = navigationService;
            _navigationService.Navigated += NavigationServiceOnNavigated;

        }

        private void NavigationServiceOnNavigated(object sender, NavigationEventArgs e)
        {
            var uri = e.Uri;

            if (uri.OriginalString.Contains("PlaceHolderMainWindowView.xaml"))
            {
                LoadingApplication = false;
            }
        }

        public string? Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            _navigationService.Navigated -= NavigationServiceOnNavigated;
            return base.OnDeactivateAsync(close, cancellationToken);
        }
    }
}
