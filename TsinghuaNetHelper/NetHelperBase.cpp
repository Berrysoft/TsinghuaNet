#include "pch.h"

#include "NetHelperBase.h"
#include <iomanip>
#include <winrt/Windows.Security.Cryptography.Core.h>
#include <winrt/Windows.Web.Http.Headers.h>

using namespace std;
using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::Security::Cryptography;
using namespace Windows::Security::Cryptography::Core;
using namespace Windows::Storage::Streams;
using namespace Windows::Web::Http;

namespace winrt::TsinghuaNetHelper
{
    wstring GetHexString(IBuffer const& buffer)
    {
        wostringstream oss;
        auto data = buffer.data();
        auto size = buffer.Length();
        for (uint32_t i = 0; i < size; i++)
        {
            oss << setw(2) << setfill(L'0') << hex << data[i];
        }
        return oss.str();
    }

    wstring GetHashString(hstring const& input, hstring const& algorithm)
    {
        if (input.empty())
            return {};
        auto hash = HashAlgorithmProvider::OpenAlgorithm(algorithm);
        auto data = hash.HashData(CryptographicBuffer::ConvertStringToBinary(input, BinaryStringEncoding::Utf8));
        return GetHexString(data);
    }

    wstring GetMD5(hstring const& input)
    {
        return GetHashString(input, HashAlgorithmNames::Md5());
    }

    wstring GetSHA1(hstring const& input)
    {
        return GetHashString(input, HashAlgorithmNames::Sha1());
    }

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
