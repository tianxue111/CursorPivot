using System;
using WindowsInput.Native;
using WindowsInput;

namespace CursorPivot_WPF
{
    internal static class MouseKeyboardSimulator
    {
        public static InputSimulator inputSimulator = new InputSimulator();

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



        const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        const uint MOUSEEVENTF_LEFTUP = 0x0004;
        const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        const uint MOUSEEVENTF_MIDDLEUP = 0x0040;

        public static void SimulateMiddleClick()
        {
            //inputSimulator.Mouse.XButtonClick((int)VirtualKeyCode.MBUTTON);
            //inputSimulator.Mouse.RightButtonClick();

            WinApi.mouse_event(MOUSEEVENTF_MIDDLEDOWN, 0, 0, 0, UIntPtr.Zero);
            //System.Threading.Thread.Sleep(10); // 延时10毫秒
            WinApi.mouse_event(MOUSEEVENTF_MIDDLEUP, 0, 0, 0, UIntPtr.Zero);
        }
    }
}
