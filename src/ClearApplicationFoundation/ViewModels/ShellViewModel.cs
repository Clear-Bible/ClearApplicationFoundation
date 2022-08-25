using System;
using Caliburn.Micro;
using ClearApplicationFoundation.Framework;

namespace ClearApplicationFoundation.ViewModels
{
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive
    {
        public ShellViewModel(IMainWindow homeViewModel)
        {
            ActiveItem = (homeViewModel as IScreen) ?? throw new InvalidOperationException();
        }
    }
}
