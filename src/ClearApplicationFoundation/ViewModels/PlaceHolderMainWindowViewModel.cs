using Caliburn.Micro;
using ClearApplicationFoundation.ViewModels.Infrastructure;

namespace ClearApplicationFoundation.ViewModels
{
    public class PlaceHolderMainWindowViewModel : Screen, IMainWindowViewModel
    {
        private string? _title;

        public PlaceHolderMainWindowViewModel()
        {
            Title = "Placeholder Main View Model";
        }

        public string? Title
        {
            get => _title;
            set => Set(ref _title,  value);
        }
    }
}
