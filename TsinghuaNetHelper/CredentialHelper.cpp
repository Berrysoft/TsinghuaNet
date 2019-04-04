#include "pch.h"

#include "CredentialHelper.h"
#include "winrt/Windows.Security.Credentials.h"

using namespace std;
using namespace linq;
using namespace winrt;
using namespace Windows::Security::Credentials;

namespace winrt::TsinghuaNetHelper::implementation
{
    constexpr wstring_view CredentialResource{ L"TsinghuaNetUWP" };

    hstring CredentialHelper::GetCredential(hstring const& username)
    {
        PasswordVault vault;
        for (auto c : vault.RetrieveAll() >>
                          where([&](PasswordCredential c) { return c.Resource() == CredentialResource && c.UserName() == username; }))
        {
            c.RetrievePassword();
            return c.Password();
        }
        return {};
    }

    void CredentialHelper::SaveCredential(hstring const& username, hstring const& password)
    {
        PasswordVault vault;
        vault.Add({ CredentialResource, username, password });
    }

    void CredentialHelper::RemoveCredential(hstring const& username)
    {
        PasswordVault vault;
        for (auto c : vault.RetrieveAll() >>
                          where([&](auto c) { return c.Resource() == CredentialResource && c.UserName() == username; }))
        {
            vault.Remove(c);
        }
    }
} // namespace winrt::TsinghuaNetHelper::implementation
