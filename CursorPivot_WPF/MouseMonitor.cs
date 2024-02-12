using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CursorPivot_WPF
{
    class MouseMonitor
    {
        public delegate int MouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        // 这里只监控鼠标的行为
        // http://msdn.microsoft.com/en-us/library/windows/desktop/ms644990(v=vs.85).aspx
        public const int WH_MOUSE_LL = 14;

        MouseProc MouseHookProcedure;

        public int MouseHookStart(MouseProc onMouseProc)
        {
            MouseHookProcedure = new MouseProc(onMouseProc);

            // 设置钩子
            return WinApi.SetWindowsHookEx(WH_MOUSE_LL, MouseHookProcedure, IntPtr.Zero, 0);
        }

        public void MouseHookStop(int hMouseHook)
        {
            if (hMouseHook != 0)
            {
                WinApi.UnhookWindowsHookEx(hMouseHook);
            }
        }
    }
}
