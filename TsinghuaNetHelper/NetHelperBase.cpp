#include "pch.h"

#include "NetHelperBase.h"
#include <winrt/Windows.Web.Http.Headers.h>

using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::Storage::Streams;
using namespace Windows::Web::Http;

namespace winrt::TsinghuaNetHelper
{
    constexpr wchar_t UserAgent[] = L"TsinghuaNetUWP";
    NetHelperBase::NetHelperBase()
    {
        auto headers = client.DefaultRequestHeaders();
        headers.UserAgent().TryParseAdd(UserAgent);
    }

    IAsyncOperation<hstring> NetHelperBase::GetAsync(Uri const uri)
    {
        co_return co_await client.GetStringAsync(uri);
    }

    IAsyncOperation<IBuffer> NetHelperBase::GetBytesAsync(Uri const uri)
    {
        co_return co_await client.GetBufferAsync(uri);
    }

    IAsyncOperation<hstring> NetHelperBase::PostAsync(Uri const uri)
    {
        auto message = HttpRequestMessage(HttpMethod::Post(), uri);
        auto response = co_await client.SendRequestAsync(message, HttpCompletionOption::ResponseContentRead);
        co_return co_await response.Content().ReadAsStringAsync();
    }

    IAsyncOperation<hstring> NetHelperBase::PostStringAsync(Uri const uri, hstring const data)
    {
        auto content = HttpStringContent(data, UnicodeEncoding::Utf8,
                                         L"application/x-www-form-urlencoded");
        auto response = co_await client.PostAsync(uri, content);
        co_return co_await response.Content().ReadAsStringAsync();
    }

    IAsyncOperation<hstring> NetHelperBase::PostMapAsync(Uri const uri, IIterable<IKeyValuePair<hstring, hstring>> const data)
    {
        auto content = HttpFormUrlEncodedContent(data);
        auto response = co_await client.PostAsync(uri, content);
        co_return co_await response.Content().ReadAsStringAsync();
    }
} // namespace winrt::TsinghuaNetHelper
