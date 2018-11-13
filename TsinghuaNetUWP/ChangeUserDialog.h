#pragma once
#include "ChangeUserDialog.g.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct ChangeUserDialog : ChangeUserDialogT<ChangeUserDialog>
    {
        ChangeUserDialog();
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct ChangeUserDialog : ChangeUserDialogT<ChangeUserDialog, implementation::ChangeUserDialog>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation
