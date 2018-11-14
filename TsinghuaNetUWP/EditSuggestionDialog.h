#pragma once
#include "EditSuggestionDialog.g.h"

#include "WlanSuggestionModel.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct EditSuggestionDialog : EditSuggestionDialogT<EditSuggestionDialog>
    {
        EditSuggestionDialog();
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct EditSuggestionDialog : EditSuggestionDialogT<EditSuggestionDialog, implementation::EditSuggestionDialog>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation
