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
using WindowsInput;
using static CursorPivot_WPF.WinApi;

namespace CursorPivot_WPF
{
    class MouseHook
    {
        private int hMouseHook;
        private MouseMonitor mouseMonitor;
        private FloatingWindow fwindow;
        private string ActionButton = "Button0";

        private bool fwindowShown = false;
        private bool mouseDown = false;
        private POINT mouseDownPosition;
        private const int DragThreshold = 10; // 定义拖动的阈值，超过该值认为是拖动
        private bool ignoreNextEvent = false;   // 处理事件的标志位

        public InputSimulator inputSimulator = new InputSimulator();

        public delegate void Dele2Main(string value1);
        public event Dele2Main SetLabel,SetLabel2;


        public void InitHook()
        {
            mouseMonitor = new MouseMonitor();
            fwindow = new FloatingWindow();
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
            // 检查是否捕捉事件
            if (ignoreNextEvent)
            {
                return CallNextHookEx(hMouseHook, nCode, wParam, lParam);
            }

            POINT pt;
            GetCursorPos(out pt);

            switch (wParam.ToInt32())
            {
                //case MouseMessage.WM_LBUTTONDOWN:


                //    break;
                //case MouseMessage.WM_RBUTTONDOWN:


                //    break;
                case MouseMessage.WM_MBUTTONDOWN:
                    mouseDown = true;
                    mouseDownPosition = pt;
                    SetLabel($"Middle Down: {pt.X},{pt.Y}");
                    //Application.Current.MainWindow.Show();
                    //Application.Current.MainWindow.Activate();

                    // 更新窗口中心位置为鼠标位置
                    fwindow.AdjustWindowPositionForDPI(pt.X, pt.Y);
                    
                    //break;

                    // 返回非零值试图阻止消息继续传递
                    return 1;

                case MouseMessage.WM_MOUSEMOVE:
                    if (!mouseDown) 
                    {
                        break;
                    }
                    // 检查窗口是否未显示
                    if (!fwindowShown)
                    {
                        // 检查是否超过拖动阈值
                        bool mouseMove = Math.Abs(pt.X - mouseDownPosition.X) > DragThreshold || 
                            Math.Abs(pt.Y - mouseDownPosition.Y) > DragThreshold;
                        if (!mouseMove)
                        {
                            break;
                        }
                        else
                        {
                            fwindow.Visibility = Visibility.Visible;
                            fwindowShown = true;
                        }
                    }
                    

                    // 将POINT结构转换为System.Windows.Point
                    Point screenPoint = new Point(pt.X, pt.Y);
                    // 将屏幕坐标转换为相对于悬浮窗的坐标
                    Point relativePoint = fwindow.PointFromScreen(screenPoint);
                    //SetLabel2(relativePoint.X + ", " + relativePoint.Y);

                    // 使用relativePoint进行命中测试
                    var hitTestResult = VisualTreeHelper.HitTest(fwindow, relativePoint);
                    if (hitTestResult != null)
                    {
                        var button = FindParent<Button>(hitTestResult.VisualHit);
                        if (button != null)
                        {
                            // 如果鼠标在特定按钮上, 操作按钮
                            ActionButton = button.Name;
                            Console.WriteLine($"hit {ActionButton}");

                        }
                        else
                        {
                            //ActionButton = "Button0";
                        }
                        SetLabel2(ActionButton);

                    }

                    break;

                case MouseMessage.WM_MBUTTONUP:
                    ignoreNextEvent = true; // 设置标志位为true，忽略执行操作中的事件，防止循环触发鼠标事件
                    mouseDown = false;
                    if (fwindowShown)
                    {
                        fwindow.Visibility = Visibility.Hidden;
                        fwindowShown = false;
                    }
                    // 启动新线程，防止阻塞UI线程
                    Task.Run(() =>
                    {
                        fwindow.PerformButtonAction(ActionButton);
                        Console.WriteLine($"mouse button up, action {ActionButton}");
                        ActionButton = "Button0";
                        ignoreNextEvent = false; // 新线程执行完毕，再重置标志位，防止循环创建线程
                    });

                    //break;

                    // 阻止消息传递
                    return 1; 
            }

            // 对于不需要拦截的消息，继续调用CallNextHookEx
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
