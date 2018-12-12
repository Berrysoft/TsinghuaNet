#include "pch.h"

#include "ChangeUserDialog.h"

using namespace winrt;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace TsinghuaNetHelper;

namespace winrt::TsinghuaNetUWP::implementation
{
    ChangeUserDialog::ChangeUserDialog(hstring const& username)
    {
        InitializeComponent();
        UnBox().Text(username);
        UsernameChangedImpl();
    }

    void ChangeUserDialog::UsernameChangedImpl()
    {
        hstring pw = CredentialHelper::GetCredential(UnBox().Text());
        PwBox().Password(pw);
        if (pw.empty())
        {
            SaveBox().IsChecked(false);
        }
        else
        {
            SaveBox().IsChecked(true);
        }
    }
} // namespace winrt::TsinghuaNetUWP::implementation
