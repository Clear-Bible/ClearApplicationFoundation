using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ClearApplicationFoundation.Framework
{
    public class FrameSet
    {
        public FrameSet()
        {
            Frame = new Frame
            {
                NavigationUIVisibility = NavigationUIVisibility.Hidden
            };
            FrameAdapter = new FrameAdapter(Frame);
        }

        public Frame Frame { get; private set; }
        public FrameAdapter FrameAdapter { get; private set; }
        public INavigationService NavigationService => FrameAdapter as INavigationService;
    }
}
