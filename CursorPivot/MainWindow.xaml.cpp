#include "pch.h"
#include "MainWindow.xaml.h"
#if __has_include("MainWindow.g.cpp")
#include "MainWindow.g.cpp"
#endif

using namespace winrt;
using namespace Microsoft::UI::Xaml;
using namespace Microsoft::UI::Xaml::Controls;
using namespace Microsoft::UI::Xaml::Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace winrt::CursorPivot::implementation
{
    MainWindow::MainWindow()
    {
        InitializeComponent();
        m_mainAppWindow = GetAppWindowForCurrentWindow();
        m_mainAppWindow.SetPresenter(AppWindowPresenterKind::Default);  //AppWindowPresenterKind::Overlapped
        m_mainAppWindow.IsShownInSwitchers(false);

        OverlappedPresenter overlappedPresenter(NULL);
        overlappedPresenter = m_mainAppWindow.Presenter().as<OverlappedPresenter>();
        overlappedPresenter.SetBorderAndTitleBar(false, false);
        overlappedPresenter.IsMaximizable(false);
        overlappedPresenter.IsMinimizable(false);
        overlappedPresenter.IsAlwaysOnTop(true);
        overlappedPresenter.IsResizable(false);


    }

    winrt::AppWindow MainWindow::GetAppWindowForCurrentWindow()
    {
        // Get access to IWindowNative
        winrt::CursorPivot::MainWindow thisWindow = *this;
        winrt::com_ptr<IWindowNative> windowNative = thisWindow.as<IWindowNative>();

        //Get the HWND for the XAML Window
        HWND hWnd;
        windowNative->get_WindowHandle(&hWnd);

        // Get the WindowId for our window
        winrt::WindowId windowId;
        windowId = winrt::GetWindowIdFromWindow(hWnd);

        // Get the AppWindow for the WindowId
        Microsoft::UI::Windowing::AppWindow appWindow = Microsoft::UI::Windowing::AppWindow::GetFromWindowId(windowId);

        return appWindow;
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
        myButton().Content(box_value(L"Clicked"));
    }
}
