#pragma once
#include "NetHelperBase.h"
#include "winrt/TsinghuaNetHelper.h"

namespace winrt::TsinghuaNetHelper
{
    struct AuthHelper : NetHelperBase
    {
        AuthHelper(int ver, hstring const& username = {}, hstring const& password = {});

        Windows::Foundation::IAsyncOperation<LogResponse> LoginAsync();
        Windows::Foundation::IAsyncOperation<LogResponse> LogoutAsync();
        Windows::Foundation::IAsyncOperation<FluxUser> FluxAsync();

    private:
        std::wstring const LogUri;
        std::wstring const FluxUri;
        std::wstring const ChallengeUri;
        int ac_id;

        concurrency::task<std::string> ChallengeAsync();
        concurrency::task<std::map<hstring, hstring>> LoginDataAsync();
        concurrency::task<std::map<hstring, hstring>> LogoutDataAsync();
    };
} // namespace winrt::TsinghuaNetHelper
