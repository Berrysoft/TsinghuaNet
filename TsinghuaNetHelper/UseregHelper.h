#pragma once
#include "UseregHelper.g.h"

#include "NetHelperBase.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct UseregHelper : UseregHelperT<UseregHelper>, NetHelperBase
    {
        UseregHelper(hstring const& username = {}, hstring const& password = {}) : NetHelperBase(username, password) {}

        Windows::Foundation::IAsyncOperation<LogResponse> LoginAsync();
        Windows::Foundation::IAsyncOperation<LogResponse> LogoutAsync();
        Windows::Foundation::IAsyncOperation<LogResponse> LogoutAsync(hstring const ip);
        Windows::Foundation::IAsyncOperation<Windows::Foundation::Collections::IVector<TsinghuaNetHelper::NetUser>> UsersAsync();
        Windows::Foundation::IAsyncOperation<Windows::Foundation::Collections::IVector<TsinghuaNetHelper::UsageDetail>> DetailsAsync();
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct UseregHelper : UseregHelperT<UseregHelper, implementation::UseregHelper>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation
