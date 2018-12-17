#pragma once
#include "NetHelperBase.h"
#include "winrt/TsinghuaNetHelper.h"
#include <ppltasks.h>

namespace winrt::TsinghuaNetHelper
{
    struct AuthHelper : NetHelperBase
    {
        AuthHelper(int ver, int ac_id = 1);

        Windows::Foundation::IAsyncOperation<LogResponse> LoginAsync();
        Windows::Foundation::IAsyncOperation<LogResponse> LogoutAsync();
        Windows::Foundation::IAsyncOperation<FluxUser> FluxAsync();

    private:
        std::wstring const LogUri;
        std::wstring const FluxUri;
        std::wstring const ChallengeUri;
        int ac_id;

        concurrency::task<std::string> ChallengeAsync();
        Windows::Foundation::IAsyncOperation<Windows::Foundation::Collections::IMap<hstring, hstring>> LoginDataAsync();
        Windows::Foundation::IAsyncOperation<Windows::Foundation::Collections::IMap<hstring, hstring>> LogoutDataAsync();
    };
} // namespace winrt::TsinghuaNetHelper
