#pragma once
#include "NetHelperBase.h"
#include "winrt/TsinghuaNetHelper.h"
#include <ppltasks.h>

namespace winrt::TsinghuaNetHelper
{
    struct AuthHelper : NetHelperBase, winrt::implements<AuthHelper, IConnect>
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
} // namespace winrt::TsinghuaNetHelper
