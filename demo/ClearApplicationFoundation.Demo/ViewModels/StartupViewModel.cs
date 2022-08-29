
using Caliburn.Micro;

namespace ClearApplicationFoundation.Demo.ViewModels
{
    public class StartupViewModel : Screen
    {
      
        public StartupViewModel(INavigationService navigationService)
        {
          
            CanOk = true;
            DisplayName = "Startup Dialog";
        }


        public bool CanCancel => true /* can always cancel */;
        public async void Cancel()
        {
           

        
            await TryCloseAsync(false);
        }


        private bool _canOk;
        public bool CanOk
        {
            get => _canOk;
            set => Set(ref _canOk, value);
        }

        public async void Ok()
        {
            await TryCloseAsync(true);
        }



    }
}
