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

    //    // ����Բ��Visual
    //    auto circleVisual = compositor.CreateSpriteVisual();
    //    circleVisual.Size({ 100.0f, 100.0f }); // ���ô�С
    //    circleVisual.Brush(compositor.CreateColorBrush(Windows::UI::Colors::Blue())); // ������ɫ

    //    // ����Բ����״��Բ�ǣ��Դ���Բ��Ч��
    //    circleVisual.RoundedCorners({ 50.0f, 50.0f, 50.0f, 50.0f });

    //    // ��Բ��Visual��ӵ�������
    //    ElementCompositionPreview::SetElementChildVisual(*this, circleVisual);

    //    // ��������м��¼���ʾ�����룬��Ҫ����Ϊ��ȷ���¼��������룩
    //    // this->CoreWindow().PointerPressed([](auto&&, auto&& args)
    //    // {
    //    //     if(args->CurrentPoint().Properties().IsMiddleButtonPressed())
    //    //     {
    //    //         // ��ʾ�����Բ����������λ��
    //    //     }
    //    // });
    //}


    void HoverWindow::InitializeComposition()
    {
        // ��ȡCompositor
        auto compositor = this->Compositor();
        auto rootVisual = compositor.CreateContainerVisual();

        // ����ShapeVisual
        auto shapeVisual = compositor.CreateShapeVisual();
        shapeVisual.Size({ 100, 100 }); // ���óߴ�

        // ����������EllipseGeometry
        auto ellipseGeometry = compositor.CreateEllipseGeometry();
        ellipseGeometry.Center({ 50, 50 });
        ellipseGeometry.Radius({ 50, 50 });

        // ����������SpriteShape
        auto spriteShape = compositor.CreateSpriteShape(ellipseGeometry);
        spriteShape.FillBrush(compositor.CreateColorBrush(Colors::Blue()));

        // ��SpriteShape��ӵ�ShapeVisual
        shapeVisual.Shapes().Append(spriteShape);

        // ��ȡXamlRoot��Compositor
        auto xamlRoot = this->XamlRoot();
        auto targetVisual = xamlRoot.Compositor().CreateContainerVisual();

        // ��ShapeVisual��ӵ����ڵ�Visual Layer
        targetVisual.Children().InsertAtTop(shapeVisual);

        // ��ContainerVisual����ΪXamlRoot��Child Visual
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
