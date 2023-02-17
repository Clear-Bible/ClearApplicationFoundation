using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClearApplicationFoundation.Framework.Input
{
    public class MultiKeyBinding : InputBinding
    {
        [TypeConverter(typeof(MultiKeyGestureConverter))]
        public override InputGesture? Gesture
        {
            get => base.Gesture as MultiKeyGesture;
            set
            {
                if (!(value is MultiKeyGesture gesture))
                {
                    throw new ArgumentException();
                }

                base.Gesture = gesture;
            }
        }
    }
}
