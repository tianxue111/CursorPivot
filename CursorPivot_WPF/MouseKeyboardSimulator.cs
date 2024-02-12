using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;
using WindowsInput;

namespace CursorPivot_WPF
{
    internal class MouseKeyboardSimulator
    {
        static InputSimulator inputSimulator = new InputSimulator();

        public static void SimulateShortcut(params VirtualKeyCode[] keys)
        {
            //Press keys
            for (int i = 0; i < keys.Length; i++)
            {
                inputSimulator.Keyboard.KeyDown(keys[i]);
            }

            // Release keys in reverse order
            for (int i = keys.Length - 1; i >= 0; i--)
            {
                inputSimulator.Keyboard.KeyUp(keys[i]);
            }
        }
    }
}
