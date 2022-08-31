using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace ClearApplicationFoundation.Demo.Views.Shell
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView
    {

        // The enum flag for DwmSetWindowAttribute's second parameter, which tells the function what attribute to set.
        // Copied from dwmapi.h
        public enum Dwmwindowattribute
        {
            DwmwaWindowCornerPreference = 33
        }

        // The DWM_WINDOW_CORNER_PREFERENCE enum for DwmSetWindowAttribute's third parameter, which tells the function
        // what value of the enum to set.
        // Copied from dwmapi.h
        public enum DwmWindowCornerPreference
        {
            DwmwcpDefault = 0,
            DwmwcpDoNotRound = 1,
            DwmwcpRound = 2,
            DwmwcpRoundSmall = 3
        }

        // Import dwmapi.dll and define DwmSetWindowAttribute in C# corresponding to the native function.
        [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        internal static extern void DwmSetWindowAttribute(IntPtr hwnd,
            Dwmwindowattribute attribute,
            ref DwmWindowCornerPreference pvAttribute,
            uint cbAttribute);


        public ShellView()
        {
            InitializeComponent();

            RoundCorners();
        }

        private void RoundCorners()
        {
            var hWnd = new WindowInteropHelper(GetWindow(this)!).EnsureHandle();
            var preference = DwmWindowCornerPreference.DwmwcpRound;
            DwmSetWindowAttribute(hWnd, Dwmwindowattribute.DwmwaWindowCornerPreference, ref preference, sizeof(uint));
        }
    }
}
