using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CursorPivot_WPF
{
    internal static class MouseMessage
    {
        // https://learn.microsoft.com/zh-cn/windows/win32/inputdev/mouse-input-notifications

        // Mouse left button down
        public const int WM_LBUTTONDOWN = 0x201;
        // Mouse left button up
        public const int WM_LBUTTONUP = 0x202;

        // Mouse right button down
        public const int WM_RBUTTONDOWN = 0x204;
        // Mouse right button up
        public const int WM_RBUTTONUP = 0x205;

        // Mouse middle button down
        public const int WM_MBUTTONDOWN = 0x207;
        // Mouse middle button up
        public const int WM_MBUTTONUP = 0x208;

        // the first or second X button down           
        public const int WM_XBUTTONDOWN = 0x020B;
        // the first or second X button up
        public const int WM_XBUTTONUP = 0x020C;
        

        // Mouse move
        public const int WM_MOUSEMOVE = 0x200;

        // Mouse wheel scroll
        public const int WM_MOUSEWHEEL = 0x20A;

        // Mouse button double click
        public const int WM_LBUTTONDBLCLK = 0x203;

    }
}
