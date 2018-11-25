#include "pch.h"
#include "NetHelper.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    Windows::Foundation::IAsyncOperation<hstring> NetHelper::LoginAsync()
    {
        throw hresult_not_implemented();
    }

    Windows::Foundation::IAsyncOperation<hstring> NetHelper::LogoutAsync()
    {
        throw hresult_not_implemented();
    }

    Windows::Foundation::IAsyncOperation<TsinghuaNetHelper::FluxUser> NetHelper::FluxAsync()
    {
        throw hresult_not_implemented();
    }
}
