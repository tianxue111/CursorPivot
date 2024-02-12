#include "pch.h"
#include "HoverWindow.xaml.h"
#if __has_include("HoverWindow.g.cpp")
#include "HoverWindow.g.cpp"
#endif

using namespace winrt;
using namespace Microsoft::UI::Xaml;
using namespace Microsoft::UI::Xaml::Controls;
using namespace Microsoft::UI::Xaml::Navigation;
using namespace Microsoft::UI::Composition;
using namespace Windows::UI::Core;
using namespace Windows::UI;

namespace winrt::CursorPivot::implementation
{
    HoverWindow::HoverWindow() {
        InitializeComponent();

        m_hoverWindow = GetAppWindowForCurrentWindow();
        m_hoverWindow.SetPresenter(AppWindowPresenterKind::Default);  //AppWindowPresenterKind::Overlapped
        m_hoverWindow.IsShownInSwitchers(false);

        OverlappedPresenter overlappedPresenter(NULL);
        overlappedPresenter = m_hoverWindow.Presenter().as<OverlappedPresenter>();
        overlappedPresenter.SetBorderAndTitleBar(false, false);
        overlappedPresenter.IsMaximizable(false);
        overlappedPresenter.IsMinimizable(false);
        overlappedPresenter.IsAlwaysOnTop(true);
        overlappedPresenter.IsResizable(false);
    }

    winrt::AppWindow HoverWindow::GetAppWindowForCurrentWindow()
    {
        // Get access to IWindowNative
        winrt::CursorPivot::HoverWindow thisWindow = *this;
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

    //void HoverWindow::OnLaunched(LaunchActivatedEventArgs const&)
    //{
    //    auto compositor = Window::Current().Compositor();

    //    // 创建圆形Visual
    //    auto circleVisual = compositor.CreateSpriteVisual();
    //    circleVisual.Size({ 100.0f, 100.0f }); // 设置大小
    //    circleVisual.Brush(compositor.CreateColorBrush(Windows::UI::Colors::Blue())); // 设置颜色

    //    // 设置圆形形状的圆角，以创建圆形效果
    //    circleVisual.RoundedCorners({ 50.0f, 50.0f, 50.0f, 50.0f });

    //    // 将圆形Visual添加到窗口中
    //    ElementCompositionPreview::SetElementChildVisual(*this, circleVisual);

    //    // 监听鼠标中键事件（示例代码，需要调整为正确的事件监听代码）
    //    // this->CoreWindow().PointerPressed([](auto&&, auto&& args)
    //    // {
    //    //     if(args->CurrentPoint().Properties().IsMiddleButtonPressed())
    //    //     {
    //    //         // 显示或更新圆形悬浮窗口位置
    //    //     }
    //    // });
    //}


    void HoverWindow::InitializeComposition()
    {
        // 获取Compositor
        auto compositor = this->Compositor();
        auto rootVisual = compositor.CreateContainerVisual();

        // 创建ShapeVisual
        auto shapeVisual = compositor.CreateShapeVisual();
        shapeVisual.Size({ 100, 100 }); // 设置尺寸

        // 创建并配置EllipseGeometry
        auto ellipseGeometry = compositor.CreateEllipseGeometry();
        ellipseGeometry.Center({ 50, 50 });
        ellipseGeometry.Radius({ 50, 50 });

        // 创建并配置SpriteShape
        auto spriteShape = compositor.CreateSpriteShape(ellipseGeometry);
        spriteShape.FillBrush(compositor.CreateColorBrush(Colors::Blue()));

        // 将SpriteShape添加到ShapeVisual
        shapeVisual.Shapes().Append(spriteShape);

        // 获取XamlRoot的Compositor
        auto xamlRoot = this->XamlRoot();
        auto targetVisual = xamlRoot.Compositor().CreateContainerVisual();

        // 将ShapeVisual添加到窗口的Visual Layer
        targetVisual.Children().InsertAtTop(shapeVisual);

        // 将ContainerVisual设置为XamlRoot的Child Visual
        auto xamlRootVisual = xamlRoot.Visual();
        xamlRootVisual.Children().InsertAtTop(targetVisual);

        rootVisual.Children().InsertAtTop(shapeVisual);

    }

    int32_t HoverWindow::MyProperty()
    {
        throw hresult_not_implemented();
    }

    void HoverWindow::MyProperty(int32_t /* value */)
    {
        throw hresult_not_implemented();
    }


}
