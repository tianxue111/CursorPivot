#pragma once

#include "HoverWindow.g.h"

namespace winrt::CursorPivot::implementation
{
    struct HoverWindow : HoverWindowT<HoverWindow>
    {
        HoverWindow();

        void OnLaunched(LaunchActivatedEventArgs const&);

        void InitializeComposition();

        int32_t MyProperty();
        void MyProperty(int32_t value);


    private:
        winrt::AppWindow GetAppWindowForCurrentWindow();
        winrt::AppWindow m_hoverWindow{ nullptr };
    };
}

namespace winrt::CursorPivot::factory_implementation
{
    struct HoverWindow : HoverWindowT<HoverWindow, implementation::HoverWindow>
    {
    };
}
