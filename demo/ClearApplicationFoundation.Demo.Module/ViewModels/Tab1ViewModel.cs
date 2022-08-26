
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using ClearApplicationFoundation.Framework;

namespace ClearApplicationFoundation.Demo.Module.ViewModels
{
    public class Tab1ViewModel : Screen, ITab
    {
        public Tab1ViewModel()
        {
            DisplayName = "Tab One";
        }

        protected override Task OnInitializeAsync(CancellationToken cancellationToken)
        {
          
            return base.OnInitializeAsync(cancellationToken);
        }

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
           return base.OnActivateAsync(cancellationToken);
        }
    }
}
