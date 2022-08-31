using Caliburn.Micro;
using ClearApplicationFoundation.ViewModels.Shell;

namespace ClearApplicationFoundation.Demo.ViewModels.Shell
{
    public class ShellViewModel : Screen, IShellViewModel
    {

        public ShellViewModel()
        {
            Title = "Foundation Demo Application";
        }

        private string? _title;
        public string? Title
        {
            get => _title;
            set => Set(ref _title, value);
        }
    }
}
