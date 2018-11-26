#pragma once
#include "AuthHelper.g.h"

#include "NetHelperBase.h"
#include <ppltasks.h>

namespace winrt::TsinghuaNetHelper::implementation
{
    struct AuthHelper : AuthHelperT<AuthHelper, TsinghuaNetHelper::implementation::NetHelperBase>
    {
        AuthHelper() = delete;
        AuthHelper(int ver);

        Windows::Foundation::IAsyncOperation<hstring> LoginAsync();
        Windows::Foundation::IAsyncOperation<hstring> LogoutAsync();
        Windows::Foundation::IAsyncOperation<TsinghuaNetHelper::FluxUser> FluxAsync();

    private:
        std::wstring const LogUri;
        std::wstring const FluxUri;
        std::wstring const ChallengeUri;

        concurrency::task<std::string> ChallengeAsync();
        Windows::Foundation::IAsyncOperation<Windows::Foundation::Collections::IMap<winrt::hstring, winrt::hstring>> LoginDataAsync();
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct AuthHelper : AuthHelperT<AuthHelper, implementation::AuthHelper>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation
