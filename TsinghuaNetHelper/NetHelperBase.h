#pragma once
#include "../Shared/Utility.h"

namespace winrt::TsinghuaNetHelper
{
    struct NetHelperBase
    {
        NetHelperBase() = default;

        Windows::Foundation::IAsyncOperation<hstring> GetAsync(Windows::Foundation::Uri const uri);
        Windows::Foundation::IAsyncOperation<Windows::Storage::Streams::IBuffer> GetBytesAsync(Windows::Foundation::Uri const uri);
        Windows::Foundation::IAsyncOperation<hstring> PostAsync(Windows::Foundation::Uri const uri);
        Windows::Foundation::IAsyncOperation<hstring> PostStringAsync(Windows::Foundation::Uri const uri, hstring const data);
        Windows::Foundation::IAsyncOperation<hstring> PostMapAsync(Windows::Foundation::Uri const uri, Windows::Foundation::Collections::IIterable<Windows::Foundation::Collections::IKeyValuePair<hstring, hstring>> const data);

        PROP_DECL_REF(Username, hstring)
        PROP_DECL_REF(Password, hstring)

    private:
        static Windows::Web::Http::HttpClient client;
    };
} // namespace winrt::TsinghuaNetHelper
