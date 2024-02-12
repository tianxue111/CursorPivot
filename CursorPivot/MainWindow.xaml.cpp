#include "pch.h"
#include "MainWindow.xaml.h"
#include "HoverWindow.xaml.h"
#if __has_include("MainWindow.g.cpp")
#include "MainWindow.g.cpp"
#endif

using namespace winrt;
using namespace Microsoft::UI::Xaml;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace winrt::CursorPivot::implementation
{
    MainWindow::MainWindow()
    {
        //InitializeComponent();

    }

    int32_t MainWindow::MyProperty()
    {
        throw hresult_not_implemented();
    }

    void MainWindow::MyProperty(int32_t /* value */)
    {
        throw hresult_not_implemented();
    }

    void MainWindow::myButton_Click(IInspectable const&, RoutedEventArgs const&)
    {
        CenterButton().Content(box_value(L"Clicked"));
        auto hoverWindow = make<HoverWindow>();
        hoverWindow.Activate();

    }

}
