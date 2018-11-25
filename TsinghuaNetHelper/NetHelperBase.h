#pragma once

#include "NetHelperBase.g.h"

namespace winrt::TsinghuaNetHelper::implementation
{
    struct NetHelperBase : NetHelperBaseT<NetHelperBase>
    {
        NetHelperBase() = delete;

        hstring Username();
        void Username(hstring const& value);
        hstring Password();
        void Password(hstring const& value);
        Windows::Foundation::IAsyncOperation<hstring> GetAsync(Windows::Foundation::Uri const uri);
        Windows::Foundation::IAsyncOperation<Windows::Storage::Streams::IBuffer> GetBytesAsync(Windows::Foundation::Uri const uri);
        Windows::Foundation::IAsyncOperation<hstring> PostAsync(Windows::Foundation::Uri const uri);
        Windows::Foundation::IAsyncOperation<hstring> PostStringAsync(Windows::Foundation::Uri const uri, hstring const data);
        Windows::Foundation::IAsyncOperation<hstring> PostMapAsync(Windows::Foundation::Uri const uri, Windows::Foundation::Collections::IIterable<Windows::Foundation::Collections::IKeyValuePair<hstring, hstring>> const data);
    };
}
