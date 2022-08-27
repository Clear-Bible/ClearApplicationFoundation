using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace ClearApplicationFoundation.Windows
{
    public class CustomizableChromeWindow : Window, INotifyPropertyChanged
    {

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
            OnPropertyChanged(nameof(CaptionButtonMargin));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            HandleSizeToContent();
        }
        private void HandleSizeToContent()
        {
            if (this.SizeToContent == SizeToContent.Manual)
                return;

            var previousTopXPosition = this.Left;
            var previousTopYPosition = this.Top;
            var previousWidth = this.MaxWidth;
            var previousHeight = this.MaxHeight;

            var previousWindowStartupLocation = this.WindowStartupLocation;
            var previousSizeToContent = SizeToContent;
            SizeToContent = SizeToContent.Manual;
            Dispatcher.BeginInvoke(
            DispatcherPriority.Loaded,
            (Action)(() =>
            {
                this.SizeToContent = previousSizeToContent;

                this.WindowStartupLocation = WindowStartupLocation.Manual;

                this.Left = previousTopXPosition + (previousWidth - this.ActualWidth) / 2;
                this.Top = previousTopYPosition + (previousHeight - this.ActualHeight) / 2;
                this.WindowStartupLocation = previousWindowStartupLocation;
            }));
        }
        public Thickness CaptionButtonMargin => new Thickness(0, 0, 0, 0);

        #region INotifyPropertyChanged
        private void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion
    }
}
