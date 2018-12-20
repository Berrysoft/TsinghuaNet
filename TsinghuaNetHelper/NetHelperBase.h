#pragma once
#include "../Shared/Utility.h"

namespace winrt::TsinghuaNetHelper
{
    struct NetHelperBase
    {
        NetHelperBase() = default;

        concurrency::task<hstring> GetAsync(Windows::Foundation::Uri const uri);
        concurrency::task<std::string> GetBytesAsync(Windows::Foundation::Uri const uri);
        concurrency::task<hstring> PostAsync(Windows::Foundation::Uri const uri);
        concurrency::task<hstring> PostAsync(Windows::Foundation::Uri const uri, hstring const data);
        concurrency::task<hstring> PostAsync(Windows::Foundation::Uri const uri, Windows::Foundation::Collections::IIterable<Windows::Foundation::Collections::IKeyValuePair<hstring, hstring>> const data);

        PROP_DECL_REF(Username, hstring)
        PROP_DECL_REF(Password, hstring)

    private:
        static Windows::Web::Http::HttpClient client;
    };
} // namespace winrt::TsinghuaNetHelper
