#include "pch.h"
#include "UseregHelper.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    Windows::Foundation::IAsyncOperation<hstring> UseregHelper::LoginAsync()
    {
        throw hresult_not_implemented();
    }

    Windows::Foundation::IAsyncOperation<hstring> UseregHelper::LogoutAsync()
    {
        throw hresult_not_implemented();
    }

    Windows::Foundation::IAsyncOperation<hstring> UseregHelper::LogoutAsync(hstring const ip)
    {
        throw hresult_not_implemented();
    }

    Windows::Foundation::IAsyncOperation<Windows::Foundation::Collections::IIterable<TsinghuaNetHelper::NetUser>> UseregHelper::UsersAsync()
    {
        throw hresult_not_implemented();
    }
}
