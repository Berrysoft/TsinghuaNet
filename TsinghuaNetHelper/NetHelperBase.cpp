#include "pch.h"
#include "NetHelperBase.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    hstring NetHelperBase::Username()
    {
        throw hresult_not_implemented();
    }

    void NetHelperBase::Username(hstring const& value)
    {
        throw hresult_not_implemented();
    }

    hstring NetHelperBase::Password()
    {
        throw hresult_not_implemented();
    }

    void NetHelperBase::Password(hstring const& value)
    {
        throw hresult_not_implemented();
    }

    Windows::Foundation::IAsyncOperation<hstring> NetHelperBase::GetAsync(Windows::Foundation::Uri const uri)
    {
        throw hresult_not_implemented();
    }

    Windows::Foundation::IAsyncOperation<Windows::Storage::Streams::IBuffer> NetHelperBase::GetBytesAsync(Windows::Foundation::Uri const uri)
    {
        throw hresult_not_implemented();
    }

    Windows::Foundation::IAsyncOperation<hstring> NetHelperBase::PostAsync(Windows::Foundation::Uri const uri)
    {
        throw hresult_not_implemented();
    }

    Windows::Foundation::IAsyncOperation<hstring> NetHelperBase::PostStringAsync(Windows::Foundation::Uri const uri, hstring const data)
    {
        throw hresult_not_implemented();
    }

    Windows::Foundation::IAsyncOperation<hstring> NetHelperBase::PostMapAsync(Windows::Foundation::Uri const uri, Windows::Foundation::Collections::IIterable<Windows::Foundation::Collections::IKeyValuePair<hstring, hstring>> const data)
    {
        throw hresult_not_implemented();
    }
}
