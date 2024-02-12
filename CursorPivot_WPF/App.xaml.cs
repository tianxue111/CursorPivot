using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms; // for NotifyIcon
using System.Drawing;
using Application = System.Windows.Application;

namespace CursorPivot_WPF
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private NotifyIcon notifyIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = new System.Drawing.Icon("./icon.ico");
            notifyIcon.Visible = true;

            // 左键点击事件
            notifyIcon.Click += Switch_MainWindow;

            // 创建上下文菜单（右键菜单）
            notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            notifyIcon.ContextMenuStrip.Items.Add("Exit", null, Exit_Click);


        }

        private void Switch_MainWindow(object sender, EventArgs e)
        {
            // Console.WriteLine(MainWindow.WindowState.ToString());
            if (MainWindow.WindowState == WindowState.Minimized)
            {
                MainWindow.Show();
                MainWindow.WindowState = WindowState.Normal;
            }
            else
            {
                MainWindow.Visibility = Visibility.Hidden;
                MainWindow.WindowState = WindowState.Minimized;
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            notifyIcon.Visible = false; // 确保在退出前隐藏图标
            Application.Current.Shutdown(); // 关闭应用程序
        }

        protected override void OnExit(ExitEventArgs e)
        {
            notifyIcon.Dispose(); // 清理图标，以免在应用程序退出后仍然留在系统托盘
            base.OnExit(e);
        }
    }
}
