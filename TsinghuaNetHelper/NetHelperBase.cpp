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
    HttpClient NetHelperBase::client = HttpClient();

    /// <summary>
    /// 从指定的URI获取字符串
    /// </summary>
    /// <param name="uri">指定的URI</param>
    IAsyncOperation<hstring> NetHelperBase::GetAsync(Uri const uri)
    {
        co_return co_await client.GetStringAsync(uri);
    }

    /// <summary>
    /// 从指定的URI获取字节Buffer
    /// </summary>
    /// <param name="uri">指定的URI</param>
    IAsyncOperation<IBuffer> NetHelperBase::GetBytesAsync(Uri const uri)
    {
        co_return co_await client.GetBufferAsync(uri);
    }

    /// <summary>
    /// 向指定的URI发送空POST
    /// </summary>
    /// <param name="uri">指定的URI</param>
    IAsyncOperation<hstring> NetHelperBase::PostAsync(Uri const uri)
    {
        auto message = HttpRequestMessage(HttpMethod::Post(), uri);
        auto response = co_await client.SendRequestAsync(message, HttpCompletionOption::ResponseContentRead);
        co_return co_await response.Content().ReadAsStringAsync();
    }

    /// <summary>
    /// 向指定的URI发送POST
    /// </summary>
    /// <param name="uri">指定的URI</param>
    /// <param name="data">要发送的数据</param>
    IAsyncOperation<hstring> NetHelperBase::PostStringAsync(Uri const uri, hstring const data)
    {
        auto content = HttpStringContent(data, UnicodeEncoding::Utf8,
                                         L"application/x-www-form-urlencoded");
        auto response = co_await client.PostAsync(uri, content);
        co_return co_await response.Content().ReadAsStringAsync();
    }

    /// <summary>
    /// 向指定的URI发送POST
    /// </summary>
    /// <param name="uri">指定的URI</param>
    /// <param name="data">要发送的数据</param>
    IAsyncOperation<hstring> NetHelperBase::PostMapAsync(Uri const uri, IIterable<IKeyValuePair<hstring, hstring>> const data)
    {
        auto content = HttpFormUrlEncodedContent(data);
        auto response = co_await client.PostAsync(uri, content);
        co_return co_await response.Content().ReadAsStringAsync();
    }
} // namespace winrt::TsinghuaNetHelper
