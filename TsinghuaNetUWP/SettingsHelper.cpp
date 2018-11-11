#include "pch.h"

#include "SettingsHelper.h"
#include <winrt/Windows.Security.Credentials.h>
#include <winrt/Windows.Storage.h>

using namespace winrt;
using namespace Windows::Security::Credentials;
using namespace Windows::Storage;

namespace winrt::TsinghuaNetUWP
{
    constexpr wchar_t CredentialResource[] = L"TsinghuaNetUWP";
    hstring GetCredential(hstring const& username)
    {
        PasswordVault vault;
        auto all = vault.RetrieveAll();
        for (auto c : all)
        {
            if (c.Resource() == CredentialResource && c.UserName() == username)
            {
                c.RetrievePassword();
                return c.Password();
            }
        }
        return {};
    }
    void SaveCredential(hstring const& username, hstring const& password)
    {
        PasswordVault vault;
        vault.Add({ CredentialResource, username, password });
    }
    void RemoveCredential(hstring const& username)
    {
        PasswordVault vault;
        auto all = vault.RetrieveAll();
        for (auto c : all)
        {
            if (c.Resource() == CredentialResource && c.UserName() == username)
            {
                vault.Remove(c);
            }
        }
    }

    constexpr wchar_t StoredUsernameKey[] = L"Username";
    hstring StoredUsername()
    {
        auto settings = ApplicationData::Current().LocalSettings();
        auto values = settings.Values();
        if (values.HasKey(StoredUsernameKey))
        {
            return unbox_value<hstring>(values.Lookup(StoredUsernameKey));
        }
        return {};
    }
    void StoredUsername(winrt::hstring const& value)
    {
        auto settings = ApplicationData::Current().LocalSettings();
        auto values = settings.Values();
        values.Insert(StoredUsernameKey, box_value(value));
    }
} // namespace winrt::TsinghuaNetUWP
