#include "pch.h"

#include "CryptographyHelper.h"
#include <winrt/Windows.Security.Cryptography.Core.h>

using namespace std;
using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::Security::Cryptography;
using namespace Windows::Security::Cryptography::Core;
using namespace Windows::Storage::Streams;

namespace winrt::TsinghuaNetHelper
{
    hstring GetHashString(hstring const& input, hstring const& algorithm)
    {
        if (input.empty())
            return {};
        auto hash = HashAlgorithmProvider::OpenAlgorithm(algorithm);
        auto data = hash.HashData(CryptographicBuffer::ConvertStringToBinary(input, BinaryStringEncoding::Utf8));
        return CryptographicBuffer::EncodeToHexString(data);
    }

    hstring GetMD5(hstring const& input)
    {
        return GetHashString(input, HashAlgorithmNames::Md5());
    }

    hstring GetSHA1(hstring const& input)
    {
        return GetHashString(input, HashAlgorithmNames::Sha1());
    }

    vector<uint32_t> S(string const& a, bool b)
    {
        uint32_t c = (uint32_t)a.length();
        uint32_t n = c / 4;
        n += c % 4 != 0 ? 1 : 0;
        vector<uint32_t> v;
        if (b)
        {
            v = vector<uint32_t>(n + 1);
            v[n] = c;
        }
        else
        {
            v = vector<uint32_t>(n < 4 ? 4 : n);
        }
        copy(a.begin(), a.end(), (char*)addressof(v.front()));
        return v;
    }

    string L(vector<uint32_t> const& a, bool b)
    {
        uint32_t d = (uint32_t)a.size();
        uint32_t c = (d - 1) << 2;
        if (b)
        {
            uint32_t m = a[d - 1];
            if (m < c - 3 || m > c)
            {
                return {};
            }
            c = m;
        }
        uint32_t n = d << 2;
        string aa(n, '\0');
        copy((char*)addressof(a.front()), (char*)addressof(a.back()) + 4, aa.begin());
        if (b)
            return aa.substr(0, c);
        else
            return aa;
    }

    string XEncode(string const& str, string const& key)
    {
        if (str.empty())
            return {};
        auto v = S(str, true);
        auto k = S(key, false);
        uint32_t n = (uint32_t)v.size() - 1;
        uint32_t z = v.back();
        uint32_t y = v.front();
        uint32_t q = 6 + 52 / (n + 1);
        uint32_t d = 0;
        while (q--)
        {
            d += 0x9E3779B9;
            uint32_t e = (d >> 2) & 3;
            for (uint32_t p = 0; p <= n; p++)
            {
                y = v[p == n ? 0 : p + 1];
                uint32_t m = (z >> 5) ^ (y << 2);
                m += (y >> 3) ^ (z << 4) ^ (d ^ y);
                m += k[(p & 3) ^ e] ^ z;
                z = v[p] += m;
            }
        }
        return L(v, false);
    }

    constexpr char Base64N[] = "LVoJPiCN2R8G90yg+hmFHuacZ1OWMnrsSTXkYpUq/3dlbfKwv6xztjI7DeBE45QA";
    string Base64Encode(string const& t)
    {
        size_t a = t.length();
        size_t len = a / 3 * 4;
        len += a % 3 != 0 ? 4 : 0;
        string u(len, L'\0');
        char r = '=';
        uint32_t h = 0;
        char* p = (char*)&h;
        size_t ui = 0;
        for (size_t o = 0; o < a; o += 3)
        {
            p[2] = t[o];
            p[1] = o + 1 < a ? t[o + 1] : 0;
            p[0] = o + 2 < a ? t[o + 2] : 0;
            for (size_t i = 0; i < 4; i++)
            {
                if (o * 8 + i * 6 > a * 8)
                {
                    u[ui++] = r;
                }
                else
                {
                    u[ui++] = Base64N[h >> 6 * (3 - i) & 0x3F];
                }
            }
        }
        return u;
    }

    hstring GetHMACMD5(hstring const& key)
    {
        auto hash = MacAlgorithmProvider::OpenAlgorithm(MacAlgorithmNames::HmacMd5());
        auto hashkey = hash.CreateKey(CryptographicBuffer::ConvertStringToBinary(key, BinaryStringEncoding::Utf8));
        return CryptographicBuffer::EncodeToHexString(CryptographicEngine::Sign(hashkey, Buffer(0U)));
    }
} // namespace winrt::TsinghuaNetHelper
