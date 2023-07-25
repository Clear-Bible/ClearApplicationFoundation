using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ClearApplicationFoundation.Controls
{
    public class HelpOverlay : ContentControl
    {
        static HelpOverlay()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HelpOverlay),
                new FrameworkPropertyMetadata(typeof(HelpOverlay)));
        }

    }
}
