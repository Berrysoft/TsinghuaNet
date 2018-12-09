#pragma once
#include "UseregHelper.g.h"

#include "NetHelperBase.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct UseregHelper : UseregHelperT<UseregHelper>, NetHelperBase
    {
        UseregHelper() = default;

        Windows::Foundation::IAsyncOperation<hstring> LoginAsync();
        Windows::Foundation::IAsyncOperation<hstring> LogoutAsync();
        Windows::Foundation::IAsyncOperation<hstring> LogoutAsync(hstring const ip);
        Windows::Foundation::IAsyncOperation<Windows::Foundation::Collections::IVector<TsinghuaNetHelper::NetUser>> UsersAsync();
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct UseregHelper : UseregHelperT<UseregHelper, implementation::UseregHelper>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation
