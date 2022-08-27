using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace ClearApplicationFoundation.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView
    {
        public ShellView()
        {
            InitializeComponent();

        }

        //protected override void OnRenderSizeChanged(SizeChangedInfo info)
        //{
        //    base.OnRenderSizeChanged(info);

        //    // This starts as false, and gets set to true in the Window.Loaded event.
        //    // It gets set back to false during the window closing event.
        //    // The window I am using this on has the min/max buttons disabled.
        //    // If you allow min/max, you might want to override the window state changed event and 
        //    // set this to false there as well.
        //    //if (!this.canAnimate) return;

        //    DoubleAnimation? animT = null;
        //    DoubleAnimation? animL = null;

        //    if (info.HeightChanged)
        //        animT = new
        //        (
        //            this.Top,
        //            this.Top + ((info.PreviousSize.Height - info.NewSize.Height) / 2),
        //            TimeSpan.FromSeconds(0.5)
        //        )
        //        {
        //            // !Important: Animation coerces the dependency property values. If you don't
        //            // specify Stop as the fill behavior, the coerced property will always report
        //            // the wrong value if you access it directly. IE, let's say the window is at
        //            // Y = 100, and the growth animation is going to cause it to be at 90.
        //            // The user then moves the window and now the true value is 150. When 
        //            // accessing this.Top the value will constantly be stuck at 90 - that is if you
        //            // don't specify FillBehavior = Stop.
        //            FillBehavior = FillBehavior.Stop
        //        };

        //    if (info.WidthChanged)
        //        animL = new
        //        (
        //            this.Left,
        //            this.Left + ((info.PreviousSize.Width - info.NewSize.Width) / 2),
        //            TimeSpan.FromSeconds(0.5)
        //        )
        //        {
        //            FillBehavior = FillBehavior.Stop
        //        };

        //    if (animT is not null)
        //        this.BeginAnimation(TopProperty, animT);

        //    if (animL is not null)
        //        this.BeginAnimation(LeftProperty, animL);

        //}
    }
}
