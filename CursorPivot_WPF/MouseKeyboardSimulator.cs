using System;
using InputSimulatorStandard;
using InputSimulatorStandard.Native;

namespace CursorPivot_WPF
{
    internal static class MouseKeyboardSimulator
    {
        public static InputSimulator inputSimulator = new InputSimulator();

        // 模拟多键快捷键
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

        //shortcuts

        public static void Simulate_LastDesktop()
        {
            SimulateShortcut(VirtualKeyCode.CONTROL, VirtualKeyCode.LWIN, VirtualKeyCode.LEFT);
        }
        public static void Simulate_NextDesktop()
        {
            SimulateShortcut(VirtualKeyCode.CONTROL, VirtualKeyCode.LWIN, VirtualKeyCode.RIGHT);
        }

        public static void Simulate_LastWindow()
        {
            SimulateShortcut(VirtualKeyCode.MENU, VirtualKeyCode.SHIFT, VirtualKeyCode.TAB);
            //switcher.SwitchToPreviousWindow();
        }

        public static void Simulate_NextWindow()
        {
            //SimulateShortcut(VirtualKeyCode.MENU, VirtualKeyCode.TAB);
            inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.TAB);

            //switcher.SwitchToNextWindow();
        }

        public static void Simulate_MinimizeAll()
        {
            //SimulateShortcut(VirtualKeyCode.LWIN, VirtualKeyCode.VK_M);
            inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_M);
        }

        public static void Simulate_Maximize()
        {
            //SimulateShortcut(VirtualKeyCode.LWIN, VirtualKeyCode.UP);
            inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.UP);
        }

        public static void Simulate_Tasks()
        {
            //SimulateShortcut(VirtualKeyCode.LWIN, VirtualKeyCode.TAB);
            inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.TAB);
        }


        //const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        //const uint MOUSEEVENTF_LEFTUP = 0x0004;
        //const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        //const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        //const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        //const uint MOUSEEVENTF_MIDDLEUP = 0x0040;

        public static void SimulateMiddleClick()
        {
            //TODO: 适配自定义触发键
            inputSimulator.Mouse.MiddleButtonClick();

            //WinApi.mouse_event(MOUSEEVENTF_MIDDLEDOWN, 0, 0, 0, UIntPtr.Zero);
            //WinApi.mouse_event(MOUSEEVENTF_MIDDLEUP, 0, 0, 0, UIntPtr.Zero);
        }

    }
}
