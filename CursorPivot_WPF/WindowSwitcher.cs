using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

public class WindowSwitcher
{
    [DllImport("user32.dll")]
    private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

    [DllImport("user32.dll")]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    [DllImport("user32.dll")]
    private static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    public void SwitchToNextWindow()
    {
        IntPtr currentWindow = GetForegroundWindow();
        List<IntPtr> windows = new List<IntPtr>();

        EnumWindows(delegate (IntPtr hWnd, IntPtr lParam)
        {
            if (hWnd != currentWindow && IsWindowVisible(hWnd))
            {
                StringBuilder text = new StringBuilder(255);
                GetWindowText(hWnd, text, 255);
                if (!string.IsNullOrWhiteSpace(text.ToString()))
                {
                    windows.Add(hWnd);
                }
            }
            return true;
        }, IntPtr.Zero);

        // Find the index of the current window in the list
        int currentIndex = windows.IndexOf(currentWindow);
        // Calculate the index of the next window to focus
        int nextIndex = (currentIndex + 1) % windows.Count;

        // Set the next window as the foreground window
        if (windows.Count > 0 && nextIndex >= 0 && nextIndex < windows.Count)
        {
            SetForegroundWindow(windows[nextIndex]);
        }
    }

    public void SwitchToPreviousWindow()
    {
        IntPtr currentWindow = GetForegroundWindow();
        List<IntPtr> windows = new List<IntPtr>();

        EnumWindows(delegate (IntPtr hWnd, IntPtr lParam)
        {
            if (hWnd != currentWindow && IsWindowVisible(hWnd))
            {
                StringBuilder text = new StringBuilder(255);
                GetWindowText(hWnd, text, 255);
                if (!string.IsNullOrWhiteSpace(text.ToString()))
                {
                    windows.Add(hWnd);
                }
            }
            return true;
        }, IntPtr.Zero);

        // Ensure the list is in the order in which windows were added
        windows.Reverse();

        // Find the index of the current window in the list
        int currentIndex = windows.IndexOf(currentWindow);
        // Calculate the index of the previous window to focus
        int previousIndex = currentIndex - 1;
        if (previousIndex < 0)
        {
            previousIndex = windows.Count - 1; // Loop back to the last window if needed
        }

        // Set the previous window as the foreground window
        if (windows.Count > 0 && previousIndex >= 0 && previousIndex < windows.Count)
        {
            SetForegroundWindow(windows[previousIndex]);
        }
    }

}
