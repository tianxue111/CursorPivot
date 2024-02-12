using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WindowsInput;
using WindowsInput.Native;
using static CursorPivot_WPF.MouseHook;

namespace CursorPivot_WPF
{
    /// <summary>
    /// FloatingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FloatingWindow : Window
    {
        private DpiScale dpi;
        public delegate void Action();
        public Action[] MyActions = new Action[13];
        WindowSwitcher switcher = new WindowSwitcher();

        public FloatingWindow()
        {
            InitializeComponent();
            //通过将窗口的扩展样式设置为包含WS_EX_TOOLWINDOW（工具窗口），窗口将不会出现在Alt+Tab的切换列表和Win+Tab的任务视图中。
            this.SourceInitialized += MainWindow_SourceInitialized;

            // 获取当前窗口的DPI信息
            //this.Activate();
            this.Show();    // TODO: 为什么先显示才能获取dpi,待研究
            dpi = GetDpi(this);
            this.Visibility = Visibility.Hidden;

            //this.Background = "Transparent";
            this.WindowStyle = WindowStyle.None;

            // 初始化代理数组
            MyActions[0] = null;
            //MyActions[1] = Button1Action;
            //MyActions[2] = Button2Action;
            MyActions[3] = Simulate_NextDesktop;
            MyActions[3] = Simulate_NextWindow;
            //MyActions[4] = Button4Action;
            //MyActions[5] = Button5Action;
            MyActions[6] = Simulate_MinimizeAll;
            //MyActions[7] = Button7Action;
            //MyActions[8] = Button8Action;
            MyActions[9] = Simulate_LastDesktop;
            MyActions[9] = Simulate_LastWindow;
            //MyActions[10] = Button10Action;
            //MyActions[11] = Button11Action;
            MyActions[12] = Simulate_Tasks;

        }

        private void Simulate_LastDesktop()
        {
            MouseKeyboardSimulator.SimulateShortcut(VirtualKeyCode.CONTROL, VirtualKeyCode.LWIN, VirtualKeyCode.LEFT);
        }
        private void Simulate_NextDesktop()
        {
            MouseKeyboardSimulator.SimulateShortcut(VirtualKeyCode.CONTROL, VirtualKeyCode.LWIN, VirtualKeyCode.RIGHT);
        }

        private void Simulate_LastWindow()
        {
            MouseKeyboardSimulator.SimulateShortcut(VirtualKeyCode.MENU, VirtualKeyCode.SHIFT, VirtualKeyCode.TAB);
            //switcher.SwitchToPreviousWindow();
        }

        private void Simulate_NextWindow()
        {
            MouseKeyboardSimulator.SimulateShortcut(VirtualKeyCode.MENU, VirtualKeyCode.TAB);
            //switcher.SwitchToNextWindow();
        }

        private void Simulate_MinimizeAll()
        {
            MouseKeyboardSimulator.SimulateShortcut(VirtualKeyCode.LWIN, VirtualKeyCode.VK_M);
            //MouseKeyboardSimulator.test();

        }

        private void Simulate_Maximize()
        {
            MouseKeyboardSimulator.SimulateShortcut(VirtualKeyCode.LWIN, VirtualKeyCode.UP);
        }

        private void Simulate_Tasks()
        {
            MouseKeyboardSimulator.SimulateShortcut(VirtualKeyCode.LWIN, VirtualKeyCode.TAB);
        }

        internal void PerformButtonAction(string buttonName)
        {
            if (buttonName == null) return;
            // 从按钮名称中提取数字, 按钮名称格式为 "buttonX"
            int index = 0;
            try
            {
                index = int.Parse(buttonName.Substring(6));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine($"button name: {buttonName}");
            }

            // 检查索引有效性
            if (index >= 0 && index < MyActions.Length && MyActions[index] != null)
            {
                MyActions[index].Invoke();
                Console.WriteLine($"Action {index} invoded");
            }
            else
            {
                Console.WriteLine($"Action {index} for {buttonName} is not defined.");
            }
        }

        private void MainWindow_SourceInitialized(object sender, EventArgs e)
        {
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            int extendedStyle = WinApi.GetWindowLong(hwnd, WinApi.GWL_EXSTYLE);
            WinApi.SetWindowLong(hwnd, WinApi.GWL_EXSTYLE, extendedStyle | WinApi.WS_EX_TOOLWINDOW);
        }

        // 调整悬浮窗位置，考虑DPI缩放
        internal void AdjustWindowPositionForDPI(double mouseX, double mouseY)
        {
            // 将屏幕坐标转换为DPI感知的坐标
            double scaledX = mouseX / dpi.DpiScaleX;
            double scaledY = mouseY / dpi.DpiScaleY;

            // 调整悬浮窗位置，使其居中于鼠标位置
            this.Left = scaledX - (this.Width / 2);
            this.Top = scaledY - (this.Height / 2);
        }

        // 获取指定窗口的DPI信息, .NET framework >= 4.6.2
        public DpiScale GetDpi(Window window)
        {
            var windowHandle = new WindowInteropHelper(window).Handle;
            var source = PresentationSource.FromVisual(window);
            // 默认DPI信息，假设为96 DPI
            //double dpiX = 96.0, dpiY = 96.0;
            double dpiScaleX = 1, dpiScaleY = 1;

            if (source != null && source.CompositionTarget != null)
            {
                dpiScaleX = source.CompositionTarget.TransformToDevice.M11;
                dpiScaleY = source.CompositionTarget.TransformToDevice.M22;
                // example: 1.5, 1.5
            }

            return new DpiScale(dpiScaleX, dpiScaleY);

        }
    }

}
