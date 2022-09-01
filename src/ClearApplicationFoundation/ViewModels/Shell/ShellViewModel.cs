using Caliburn.Micro;
using ClearApplicationFoundation.ViewModels.Infrastructure;

namespace ClearApplicationFoundation.ViewModels.Shell
{
    public class ShellViewModel : Screen, IShellViewModel
    {
        private string? _title;

        public ShellViewModel()
        {
            Title = "Clear Application Foundation";
        }

        public string? Title
        {
            get => _title;
            set => Set(ref _title, value);
        }
    }
}
