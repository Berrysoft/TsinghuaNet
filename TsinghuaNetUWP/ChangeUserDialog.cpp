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

    /// <summary>
    /// 如果已保存密码，默认勾选“保存密码”
    /// </summary>
    void ChangeUserDialog::UsernameChangedImpl()
    {
        hstring un = UnBox().Text();
        hstring pw = CredentialHelper::GetCredential(un);
        IsPrimaryButtonEnabled(!(un.empty() || pw.empty()));
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
