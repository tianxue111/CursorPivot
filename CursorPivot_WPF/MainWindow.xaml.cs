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
        public FloatingWindow floatingWindow;

        public FloatingWindow.Action[] MyActions { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            InitializeActions();
            //this.Visibility = Visibility.Hidden;
            //this.WindowState = WindowState.Minimized;


            mouseHook = new MouseHook();
            mouseHook.InitHook();
            mouseHook.FormStartHook();

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

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    //floatingWindow.Show();

        //    //Application.Current.MainWindow.Hide();

        //}


        private void InitializeActions()
        {
            // 初始化动作，类似于之前的ConfigureActions方法
            // 这里仅设置几个示例，你需要根据实际情况进行配置
            floatingWindow = new FloatingWindow();
            MyActions = floatingWindow.MyActions;
            MyActions[0] = () => MessageBox.Show("NextDesktop Simulated");
            MyActions[1] = () => MessageBox.Show("MinimizeAll Simulated");
            MyActions[2] = () => MessageBox.Show("LastDesktop Simulated");
            MyActions[3] = () => MessageBox.Show("Task Simulated");
            // 继续为每个动作赋值
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 当按钮被点击时显示下拉菜单来选择动作
            ActionSelector.Visibility = Visibility.Visible;
            ActionSelector.Tag = sender; // 将点击的按钮存储在ComboBox的Tag属性中
        }

        private void ActionSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int selectedIndex = comboBox.SelectedIndex;
            Button clickedButton = comboBox.Tag as Button;

            // 根据选择的索引为按钮绑定动作
            if (clickedButton != null && selectedIndex >= 0 && selectedIndex < MyActions.Length)
            {
                clickedButton.Click -= Button_ExecuteAction; // 移除之前可能绑定的事件
                clickedButton.Click += Button_ExecuteAction; // 添加新的事件
                clickedButton.Tag = selectedIndex; // 将动作索引存储在按钮的Tag中
                ActionSelector.Visibility = Visibility.Collapsed; // 隐藏下拉菜单
            }

        }

        private void Button_ExecuteAction(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int actionIndex = (int)button.Tag;

            // 执行绑定的动作
            MyActions[actionIndex]?.Invoke();
        }

        public int? GetActionIndexFromButtonName(string buttonName)
        {
            // 通过按钮的名字找到对应的按钮实例
            var button = this.FindName(buttonName) as Button;
            if (button == null)
            {
                // 如果没有找到按钮，返回 null
                return null;
            }

            // 尝试从按钮的 Tag 属性获取动作索引
            if (button.Tag != null && int.TryParse(button.Tag.ToString(), out int actionIndex))
            {
                // 确保索引在动作数组的有效范围内
                if (actionIndex >= 0 && actionIndex < MyActions.Length)
                {
                    return actionIndex;
                }
            }

            // 如果 Tag 不是一个有效的索引，返回 null
            return null;
        }

    }
}
