using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CursorPivot_WPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private MouseHook mouseHook;

        public MainWindow()
        {
            InitializeComponent();
            this.Visibility = Visibility.Hidden;
            this.WindowState = WindowState.Minimized;


            mouseHook = new MouseHook();
            mouseHook.InitHook();
            mouseHook.FormStartHook();
            mouseHook.SetLabel += SetMouseStateLabel;
            mouseHook.SetLabel2 += SetMouseStateLabel2;

        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            //MessageBoxResult result = MessageBox.Show("确定是退出吗？", "询问", MessageBoxButton.YesNo, MessageBoxImage.Question);
            ////关闭窗口
            //if (result == MessageBoxResult.Yes)
            //    mouseHook.FormStopHook();
            //    e.Cancel = false;
            ////不关闭窗口
            //if (result == MessageBoxResult.No)
            //    e.Cancel = true;

            mouseHook.FormStopHook();
        }

        public void SetMouseStateLabel(string value)
        {
            //this.MouseStateLabel.Content = mouseHook.mouseState.ToString();
            this.MouseStateLabel.Content = value;
        }
        public void SetMouseStateLabel2(string value)
        {
            this.MouseStateLabel2.Content = value;
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            //floatingWindow.Show();

            Application.Current.MainWindow.Hide();

        }

    }
}
