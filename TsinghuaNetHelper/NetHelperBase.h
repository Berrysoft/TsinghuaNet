#pragma once
#include "Utility.h"

namespace winrt::TsinghuaNetHelper
{
    std::wstring GetMD5(hstring const& input);
    std::wstring GetSHA1(hstring const& input);

    struct NetHelperBase
    {
        NetHelperBase();

        Windows::Foundation::IAsyncOperation<hstring> GetAsync(Windows::Foundation::Uri const uri);
        Windows::Foundation::IAsyncOperation<Windows::Storage::Streams::IBuffer> GetBytesAsync(Windows::Foundation::Uri const uri);
        Windows::Foundation::IAsyncOperation<hstring> PostAsync(Windows::Foundation::Uri const uri);
        Windows::Foundation::IAsyncOperation<hstring> PostStringAsync(Windows::Foundation::Uri const uri, hstring const data);
        Windows::Foundation::IAsyncOperation<hstring> PostMapAsync(Windows::Foundation::Uri const uri, Windows::Foundation::Collections::IIterable<Windows::Foundation::Collections::IKeyValuePair<hstring, hstring>> const data);

        PROP_DECL_REF(Username, winrt::hstring)
        PROP_DECL_REF(Password, winrt::hstring)

    private:
        Windows::Web::Http::HttpClient client;
    };
} // namespace winrt::TsinghuaNetHelper
