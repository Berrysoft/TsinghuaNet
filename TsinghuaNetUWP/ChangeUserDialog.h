#pragma once
#include "ChangeUserDialog.g.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct ChangeUserDialog : ChangeUserDialogT<ChangeUserDialog>
    {
        ChangeUserDialog(hstring const& username = {});

        void UsernameChanged(Windows::Foundation::IInspectable const&, Windows::UI::Xaml::Controls::TextChangedEventArgs const&) { UsernameChangedImpl(); }
        void PasswordChanged(Windows::Foundation::IInspectable const&, Windows::UI::Xaml::RoutedEventArgs const&) { PasswordChangedImpl(); }

    private:
        void UsernameChangedImpl();
        void PasswordChangedImpl();
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct ChangeUserDialog : ChangeUserDialogT<ChangeUserDialog, implementation::ChangeUserDialog>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation
