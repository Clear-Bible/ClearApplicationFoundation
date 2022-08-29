using Caliburn.Micro;

namespace ClearApplicationFoundation.ViewModels.Shell
{
    public class ShellViewModel : Screen
    {
        private string _title;

        public ShellViewModel()
        {
            SetDisplayName("Clear Application Foundation");
        }

        public void SetDisplayName(string displayName)
        {
            OnUIThread(() =>
            {
                Title = displayName;
            });
        }

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }
    }
}
