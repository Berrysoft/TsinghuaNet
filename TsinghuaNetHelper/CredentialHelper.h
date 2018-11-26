#pragma once
#include "CredentialHelper.g.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct CredentialHelper
    {
        CredentialHelper() = delete;

        static hstring GetCredential(hstring const& username);
        static void SaveCredential(hstring const& username, hstring const& password);
        static void RemoveCredential(hstring const& username);
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct CredentialHelper : CredentialHelperT<CredentialHelper, implementation::CredentialHelper>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation
