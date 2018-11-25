#include "pch.h"
#include "AuthHelper.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    Windows::Foundation::IAsyncOperation<hstring> AuthHelper::LoginAsync()
    {
        throw hresult_not_implemented();
    }

    Windows::Foundation::IAsyncOperation<hstring> AuthHelper::LogoutAsync()
    {
        throw hresult_not_implemented();
    }

    Windows::Foundation::IAsyncOperation<TsinghuaNetHelper::FluxUser> AuthHelper::FluxAsync()
    {
        throw hresult_not_implemented();
    }
}
