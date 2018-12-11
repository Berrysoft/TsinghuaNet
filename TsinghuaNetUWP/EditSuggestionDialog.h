#pragma once
#include "EditSuggestionDialog.g.h"

namespace winrt
{
    inline hstring to_hstring(TsinghuaNetHelper::NetState state)
    {
        return TsinghuaNetHelper::UserHelper::GetNetStateString(state);
    }
} // namespace winrt

namespace winrt::TsinghuaNetUWP::implementation
{
    struct EditSuggestionDialog : EditSuggestionDialogT<EditSuggestionDialog>
    {
        EditSuggestionDialog();

        Windows::Foundation::Collections::IObservableVector<Windows::Foundation::IInspectable> States() { return m_States; }

    private:
        Windows::Foundation::Collections::IObservableVector<Windows::Foundation::IInspectable> m_States;
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct EditSuggestionDialog : EditSuggestionDialogT<EditSuggestionDialog, implementation::EditSuggestionDialog>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation
