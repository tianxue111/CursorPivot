using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using static CursorPivot_WPF.WinApi;

namespace CursorPivot_WPF
{
    class MouseHook
    {
        private int hMouseHook;
        private MouseMonitor mouseMonitor;
        private FloatingWindow fwindow;
        private bool fwindowShown;
        private int mouseState;
        private string ActionButton = null;

        public delegate void Dele2Main(string value1);
        public event Dele2Main SetLabel,SetLabel2;


        public void InitHook()
        {
            mouseMonitor = new MouseMonitor();
            fwindow = new FloatingWindow();
            fwindowShown = false;


        }
        public void FormStartHook()
        {
            this.hMouseHook = mouseMonitor.MouseHookStart(OnMouseProc);
        }
        public void FormStopHook()
        {
            if (this.hMouseHook != 0)
            {
                WinApi.UnhookWindowsHookEx(this.hMouseHook);
                //this.state.saveAction(DateTime.Today);
            }
            fwindow.Close();
        }

        public int OnMouseProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            POINT pt;
            GetCursorPos(out pt);

            SetLabel(pt.X+","+pt.Y);

            switch (wParam.ToInt32())
            {
                //case MouseMessage.WM_LBUTTONDOWN:


                //    break;
                //case MouseMessage.WM_RBUTTONDOWN:


                //    break;
                case MouseMessage.WM_MBUTTONDOWN:
                    mouseState += 1;
                    fwindowShown = true;
                    //Application.Current.MainWindow.Show();
                    //Application.Current.MainWindow.Activate();

                    // Set the window's position
                    fwindow.AdjustWindowPositionForDPI(pt.X, pt.Y);
                    //SetLabel2(fwindow.Left + "," + fwindow.Top);

                    fwindow.Visibility = Visibility.Visible;
                    break;

                case MouseMessage.WM_MOUSEMOVE:
                    if (fwindowShown)
                    {
                        // 将POINT结构转换为System.Windows.Point
                        Point screenPoint = new Point(pt.X, pt.Y);
                        // 将屏幕坐标转换为相对于悬浮窗的坐标
                        Point relativePoint = fwindow.PointFromScreen(screenPoint);
                        //SetLabel2(relativePoint.X + ", " + relativePoint.Y);

                        // 使用relativePoint进行后续处理，例如命中测试
                        var hitTestResult = VisualTreeHelper.HitTest(fwindow, relativePoint);
                        if (hitTestResult != null)
                        {
                            var button = FindParent<Button>(hitTestResult.VisualHit);
                            if (button != null)
                            {
                                // 如果鼠标在特定按钮上, 操作按钮
                                ActionButton = button.Name;
                                SetLabel("hit!");

                            }
                            else
                            {
                                //ActionButton = null;
                            }
                            SetLabel2(ActionButton);

                        }
                    }

                    break;

                case MouseMessage.WM_MBUTTONUP:
                    fwindowShown = false;
                    fwindow.Visibility = Visibility.Hidden;
                    
                    Console.WriteLine($"M button up, action {ActionButton}");
                    fwindow.PerformButtonAction(ActionButton);
                    ActionButton = null;

                    break;
            }
            //MainWindow.MouseStateLabel.Content = this.mouseState.ToString();

            //this.state.recordAction(wParam.ToInt32());
            return WinApi.CallNextHookEx(this.hMouseHook, nCode, wParam, lParam);
        }

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;

            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return FindParent<T>(parentObject);
            }
        }


    }
}
