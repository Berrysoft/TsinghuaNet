#pragma once
#include "NetHelperBase.h"
#include "winrt/TsinghuaNetHelper.h"
#include <ppltasks.h>

namespace winrt::TsinghuaNetHelper
{
    struct AuthHelper : NetHelperBase, winrt::implements<AuthHelper, IConnect>
    {
        AuthHelper() = delete;
        AuthHelper(int ver, int ac_id = 1);

        Windows::Foundation::IAsyncOperation<hstring> LoginAsync();
        Windows::Foundation::IAsyncOperation<hstring> LogoutAsync();
        Windows::Foundation::IAsyncOperation<TsinghuaNetHelper::FluxUser> FluxAsync();

    private:
        std::wstring const LogUri;
        std::wstring const FluxUri;
        std::wstring const ChallengeUri;
        int ac_id;

        concurrency::task<std::map<std::string, std::string>> ChallengeAsync();
        Windows::Foundation::IAsyncOperation<Windows::Foundation::Collections::IMap<winrt::hstring, winrt::hstring>> LoginDataAsync();
        Windows::Foundation::IAsyncOperation<Windows::Foundation::Collections::IMap<winrt::hstring, winrt::hstring>> LogoutDataAsync();
    };
} // namespace winrt::TsinghuaNetHelper
