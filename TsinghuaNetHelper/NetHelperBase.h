#pragma once
#include "NetHelperBase.g.h"

#include "Utility.h"

namespace winrt
{
    inline std::wostream& operator<<(std::wostream& os, hstring const& s)
    {
        return os << std::wstring_view(s);
    }
    inline std::ostream& operator<<(std::ostream& os, hstring const& s)
    {
        return os << to_string(s);
    }

    namespace TsinghuaNetHelper
    {
        std::wstring GetMD5(hstring const& input);
        std::wstring GetSHA1(hstring const& input);
    } // namespace TsinghuaNetHelper
} // namespace winrt

namespace winrt::TsinghuaNetHelper::implementation
{
    struct NetHelperBase : NetHelperBaseT<NetHelperBase>
    {
        NetHelperBase() = default;

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
} // namespace winrt::TsinghuaNetHelper::implementation

namespace winrt::TsinghuaNetHelper::factory_implementation
{
    struct NetHelperBase : NetHelperBaseT<NetHelperBase, implementation::NetHelperBase>
    {
    };
} // namespace winrt::TsinghuaNetHelper::factory_implementation
