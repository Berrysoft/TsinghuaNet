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

    vector<uint32_t> RStr2Binl(string const& input)
    {
        size_t len = input.length();
        vector<uint32_t> output((len + 3) / 4);
        copy(input.begin(), input.end(), (char*)addressof(output.front()));
        return output;
    }

    string Binl2RStr(vector<uint32_t> const& input)
    {
        size_t l = input.size() * 4;
        string result(l, '\0');
        copy((char*)addressof(input.front()), (char*)addressof(input.back()) + 4, result.begin());
        return result;
    }

    vector<uint32_t> Binl(vector<uint32_t> x, size_t len)
    {
        int32_t a = 1732584193, b = -271733879, c = -1732584194, d = 271733878;
        size_t index = len >> 5;
        while (x.size() <= index)
            x.push_back(0);
        x[index] |= 0x80 << (len % 32);
        index = (((len + 64) >> 9) << 4) + 14;
        while (x.size() < index)
            x.push_back(0);
        x.push_back((uint32_t)len);
        while (x.size() % 16)
            x.push_back(0);

        auto bit_rol = [](uint32_t num, int32_t cnt) { return (int32_t)((num << cnt) | (num >> (32 - cnt))); };
        auto md5_cmn = [&bit_rol](int32_t q, int32_t a, int32_t b, int32_t x, int32_t s, int32_t t) { return bit_rol((uint32_t)(a + q + x + t), s) + b; };
        auto md5_ff = [&md5_cmn](int32_t a, int32_t b, int32_t c, int32_t d, uint32_t x, int32_t s, int32_t t) { return md5_cmn((b & c) | ((~b) & d), a, b, (int32_t)x, s, t); };
        auto md5_gg = [&md5_cmn](int32_t a, int32_t b, int32_t c, int32_t d, uint32_t x, int32_t s, int32_t t) { return md5_cmn((b & d) | (c & (~d)), a, b, (int32_t)x, s, t); };
        auto md5_hh = [&md5_cmn](int32_t a, int32_t b, int32_t c, int32_t d, uint32_t x, int32_t s, int32_t t) { return md5_cmn(b ^ c ^ d, a, b, (int32_t)x, s, t); };
        auto md5_ii = [&md5_cmn](int32_t a, int32_t b, int32_t c, int32_t d, uint32_t x, int32_t s, int32_t t) { return md5_cmn(c ^ (b | (~d)), a, b, (int32_t)x, s, t); };

        for (size_t i = 0; i < x.size(); i += 16)
        {
            int32_t olda = a, oldb = b, oldc = c, oldd = d;

            a = md5_ff(a, b, c, d, x[i + 0], 7, -680876936);
            d = md5_ff(d, a, b, c, x[i + 1], 12, -389564586);
            c = md5_ff(c, d, a, b, x[i + 2], 17, 606105819);
            b = md5_ff(b, c, d, a, x[i + 3], 22, -1044525330);
            a = md5_ff(a, b, c, d, x[i + 4], 7, -176418897);
            d = md5_ff(d, a, b, c, x[i + 5], 12, 1200080426);
            c = md5_ff(c, d, a, b, x[i + 6], 17, -1473231341);
            b = md5_ff(b, c, d, a, x[i + 7], 22, -45705983);
            a = md5_ff(a, b, c, d, x[i + 8], 7, 1770035416);
            d = md5_ff(d, a, b, c, x[i + 9], 12, -1958414417);
            c = md5_ff(c, d, a, b, x[i + 10], 17, -42063);
            b = md5_ff(b, c, d, a, x[i + 11], 22, -1990404162);
            a = md5_ff(a, b, c, d, x[i + 12], 7, 1804603682);
            d = md5_ff(d, a, b, c, x[i + 13], 12, -40341101);
            c = md5_ff(c, d, a, b, x[i + 14], 17, -1502002290);
            b = md5_ff(b, c, d, a, x[i + 15], 22, 1236535329);

            a = md5_gg(a, b, c, d, x[i + 1], 5, -165796510);
            d = md5_gg(d, a, b, c, x[i + 6], 9, -1069501632);
            c = md5_gg(c, d, a, b, x[i + 11], 14, 643717713);
            b = md5_gg(b, c, d, a, x[i + 0], 20, -373897302);
            a = md5_gg(a, b, c, d, x[i + 5], 5, -701558691);
            d = md5_gg(d, a, b, c, x[i + 10], 9, 38016083);
            c = md5_gg(c, d, a, b, x[i + 15], 14, -660478335);
            b = md5_gg(b, c, d, a, x[i + 4], 20, -405537848);
            a = md5_gg(a, b, c, d, x[i + 9], 5, 568446438);
            d = md5_gg(d, a, b, c, x[i + 14], 9, -1019803690);
            c = md5_gg(c, d, a, b, x[i + 3], 14, -187363961);
            b = md5_gg(b, c, d, a, x[i + 8], 20, 1163531501);
            a = md5_gg(a, b, c, d, x[i + 13], 5, -1444681467);
            d = md5_gg(d, a, b, c, x[i + 2], 9, -51403784);
            c = md5_gg(c, d, a, b, x[i + 7], 14, 1735328473);
            b = md5_gg(b, c, d, a, x[i + 12], 20, -1926607734);

            a = md5_hh(a, b, c, d, x[i + 5], 4, -378558);
            d = md5_hh(d, a, b, c, x[i + 8], 11, -2022574463);
            c = md5_hh(c, d, a, b, x[i + 11], 16, 1839030562);
            b = md5_hh(b, c, d, a, x[i + 14], 23, -35309556);
            a = md5_hh(a, b, c, d, x[i + 1], 4, -1530992060);
            d = md5_hh(d, a, b, c, x[i + 4], 11, 1272893353);
            c = md5_hh(c, d, a, b, x[i + 7], 16, -155497632);
            b = md5_hh(b, c, d, a, x[i + 10], 23, -1094730640);
            a = md5_hh(a, b, c, d, x[i + 13], 4, 681279174);
            d = md5_hh(d, a, b, c, x[i + 0], 11, -358537222);
            c = md5_hh(c, d, a, b, x[i + 3], 16, -722521979);
            b = md5_hh(b, c, d, a, x[i + 6], 23, 76029189);
            a = md5_hh(a, b, c, d, x[i + 9], 4, -640364487);
            d = md5_hh(d, a, b, c, x[i + 12], 11, -421815835);
            c = md5_hh(c, d, a, b, x[i + 15], 16, 530742520);
            b = md5_hh(b, c, d, a, x[i + 2], 23, -995338651);

            a = md5_ii(a, b, c, d, x[i + 0], 6, -198630844);
            d = md5_ii(d, a, b, c, x[i + 7], 10, 1126891415);
            c = md5_ii(c, d, a, b, x[i + 14], 15, -1416354905);
            b = md5_ii(b, c, d, a, x[i + 5], 21, -57434055);
            a = md5_ii(a, b, c, d, x[i + 12], 6, 1700485571);
            d = md5_ii(d, a, b, c, x[i + 3], 10, -1894986606);
            c = md5_ii(c, d, a, b, x[i + 10], 15, -1051523);
            b = md5_ii(b, c, d, a, x[i + 1], 21, -2054922799);
            a = md5_ii(a, b, c, d, x[i + 8], 6, 1873313359);
            d = md5_ii(d, a, b, c, x[i + 15], 10, -30611744);
            c = md5_ii(c, d, a, b, x[i + 6], 15, -1560198380);
            b = md5_ii(b, c, d, a, x[i + 13], 21, 1309151649);
            a = md5_ii(a, b, c, d, x[i + 4], 6, -145523070);
            d = md5_ii(d, a, b, c, x[i + 11], 10, -1120210379);
            c = md5_ii(c, d, a, b, x[i + 2], 15, 718787259);
            b = md5_ii(b, c, d, a, x[i + 9], 21, -343485551);

            a += olda;
            b += oldb;
            c += oldc;
            d += oldd;
        }
        return { (uint32_t)a, (uint32_t)b, (uint32_t)c, (uint32_t)d };
    }

    vector<uint32_t>& concat(vector<uint32_t>& v1, vector<uint32_t> const& v2)
    {
        for (uint32_t item : v2)
        {
            v1.push_back(item);
        }
        return v1;
    }

    hstring GetHMACMD5(string const& key, string const& str)
    {
        auto bkey = RStr2Binl(key);
        if (bkey.size() > 16)
        {
            bkey = Binl(bkey, key.length() * 8);
        }
        else
        {
            while (bkey.size() < 16)
                bkey.push_back(0);
        }
        vector<uint32_t> ipad(16, 0);
        vector<uint32_t> opad(16, 0);
        for (int i = 0; i < 16; i++)
        {
            ipad[i] = bkey[i] ^ 0x36363636;
            opad[i] = bkey[i] ^ 0x5C5C5C5C;
        }
        vector<uint32_t> hash = Binl(concat(ipad, RStr2Binl(str)), 512 + str.length() * 8);
        string result = Binl2RStr(Binl(concat(opad, hash), 512 + 128));
        return CryptographicBuffer::EncodeToHexString(CryptographicBuffer::CreateFromByteArray(array_view<const uint8_t>((uint8_t*)addressof(result.front()), (uint8_t*)addressof(result.back()) + 1)));
    }
} // namespace winrt::TsinghuaNetHelper
