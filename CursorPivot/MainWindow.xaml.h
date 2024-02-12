#pragma once

#include "MainWindow.g.h"

namespace winrt::CursorPivot::implementation
{
	struct MainWindow : MainWindowT<MainWindow>
	{
	public:
		MainWindow();

		int32_t MyProperty();
		void MyProperty(int32_t value);

		void myButton_Click(IInspectable const& sender, Microsoft::UI::Xaml::RoutedEventArgs const& args);

	};
}

namespace winrt::CursorPivot::factory_implementation
{
	struct MainWindow : MainWindowT<MainWindow, implementation::MainWindow>
	{
	};
}
