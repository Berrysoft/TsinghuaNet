#pragma once
#include "UseregHelper.g.h"

#include "NetHelperBase.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct UseregHelper : UseregHelperT<UseregHelper>
    {
        UseregHelper() = default;

        winrt::hstring Username() { return base.Username(); }
        void Username(winrt::hstring const& value) { base.Username(value); }
        winrt::hstring Password() { return base.Password(); }
        void Password(winrt::hstring const& value) { base.Password(value); }

        Windows::Foundation::IAsyncOperation<hstring> LoginAsync();
        Windows::Foundation::IAsyncOperation<hstring> LogoutAsync();
        Windows::Foundation::IAsyncOperation<hstring> LogoutAsync(hstring const ip);
        Windows::Foundation::IAsyncOperation<Windows::Foundation::Collections::IVector<TsinghuaNetHelper::NetUser>> UsersAsync();

    private:
        NetHelperBase base;
    };
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct UseregHelper : UseregHelperT<UseregHelper, implementation::UseregHelper>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation
