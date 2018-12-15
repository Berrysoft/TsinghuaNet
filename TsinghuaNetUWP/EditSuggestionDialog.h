#pragma once
#include "EditSuggestionDialog.g.h"

#include "DependencyHelper.h"
#include "NetStateSsidBox.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct EditSuggestionDialog : EditSuggestionDialogT<EditSuggestionDialog>
    {
        EditSuggestionDialog();

        Windows::Foundation::Collections::IObservableVector<Windows::Foundation::IInspectable> WlanList() { return m_WlanList; }

		Windows::Foundation::IAsyncAction AddSelection(Windows::Foundation::IInspectable const, Windows::UI::Xaml::RoutedEventArgs const);
		void DeleteSelection(Windows::Foundation::IInspectable const&, Windows::UI::Xaml::RoutedEventArgs const&);

    private:
        Windows::Foundation::Collections::IObservableVector<Windows::Foundation::IInspectable> m_WlanList;
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct EditSuggestionDialog : EditSuggestionDialogT<EditSuggestionDialog, implementation::EditSuggestionDialog>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation
