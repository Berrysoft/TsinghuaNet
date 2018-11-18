#include "pch.h"

#include "NetHelper.h"
#include <iomanip>
#include <pplawait.h>
#include <regex>
#include <sf/sformat.hpp>
#include <sstream>
#include <winrt/Windows.Security.Cryptography.Core.h>
#include <winrt/Windows.Storage.Streams.h>
#include <winrt/Windows.Web.Http.h>

using sf::sprint;
using std::chrono::seconds;
using namespace std;
using namespace concurrency;
using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::Web::Http;
using namespace Windows::Security::Cryptography;
using namespace Windows::Security::Cryptography::Core;
using namespace Windows::Storage::Streams;

namespace winrt
{
    wostream& operator<<(wostream& os, hstring const& s)
    {
        return os << wstring_view(s);
    }
    ostream& operator<<(ostream& os, hstring const& s)
    {
        return os << to_string(s);
    }
} // namespace winrt

namespace winrt::TsinghuaNetUWP
{
    vector<wstring_view> string_split(wstring_view const& s, wchar_t separator)
    {
        vector<wstring_view> result;
        size_t offset = 0, index = 0;
        while ((index = s.find(separator, offset)) != wstring_view::npos)
        {
            if (index > offset)
            {
                result.push_back(s.substr(offset, index - offset));
            }
            offset = index + 1;
        }
        if (offset + 1 < s.length())
        {
            result.push_back(s.substr(offset));
        }
        return result;
    }
    FluxUser make_FluxUser(wstring_view const& fluxstr)
    {
        auto r = string_split(fluxstr, ',');
        if (!r.empty())
        {
            return { wstring(r[0]),
                     stoull(wstring(r[6])),
                     seconds(stoll(wstring(r[2])) - stoll(wstring(r[1]))),
                     stod(wstring(r[10])) };
        }
        return {};
    }

    task<hstring> NetHelper_base::GetAsync(Uri const& uri) const
    {
        return co_await client.GetStringAsync(uri);
    }
    task<IBuffer> NetHelper_base::GetBytesAsync(Uri const& uri) const
    {
        return co_await client.GetBufferAsync(uri);
    }
    task<hstring> NetHelper_base::PostAsync(Uri const& uri) const
    {
        auto message = HttpRequestMessage(HttpMethod::Post(), uri);
        auto response = co_await client.SendRequestAsync(message, HttpCompletionOption::ResponseContentRead);
        return co_await response.Content().ReadAsStringAsync();
    }
    task<hstring> NetHelper_base::PostAsync(Uri const& uri, param::hstring const& data) const
    {
        auto content = HttpStringContent(data, UnicodeEncoding::Utf8,
                                         L"application/x-www-form-urlencoded");
        auto response = co_await client.PostAsync(uri, content);
        return co_await response.Content().ReadAsStringAsync();
    }
    task<hstring> NetHelper_base::PostAsync(Uri const& uri, map<hstring, hstring> const& data) const
    {
        auto content = HttpFormUrlEncodedContent(data);
        auto response = co_await client.PostAsync(uri, content);
        return co_await response.Content().ReadAsStringAsync();
    }

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

    task<hstring> NetHelper::LoginAsync() const
    {
        auto data = sprint(LoginData, username, GetMD5(password));
        return PostAsync(Uri(LogUri), data);
    }
    task<hstring> NetHelper::LogoutAsync() const
    {
        return PostAsync(Uri(LogUri), LogoutData);
    }
    task<FluxUser> NetHelper::FluxAsync() const
    {
        return make_FluxUser(co_await PostAsync(Uri(FluxUri)));
    }

    AuthHelper::AuthHelper(int version)
        : LogUri(sprint(LogUriBase, version)),
          FluxUri(sprint(FluxUriBase, version)),
          ChallengeUri(sprint(ChallengeUriBase, version))
    {
    }
    task<hstring> AuthHelper::LoginAsync() const
    {
        auto data = co_await LoginDataAsync();
        auto result = co_await PostAsync(Uri(LogUri), data);
        return result;
    }
    task<hstring> AuthHelper::LogoutAsync() const
    {
        return PostAsync(Uri(LogUri), LogoutData);
    }
    task<FluxUser> AuthHelper::FluxAsync() const
    {
        return make_FluxUser(co_await PostAsync(Uri(FluxUri)));
    }

    constexpr char ChallengeRegex[] = "\"challenge\":\"(.*?)\"";
    task<string> AuthHelper::ChallengeAsync() const
    {
        auto bytes = co_await GetBytesAsync(Uri(sprint(ChallengeUri, username)));
        string result((const char*)bytes.data(), bytes.Length());
        regex reg(ChallengeRegex);
        smatch match;
        if (regex_search(result, match, reg))
        {
            return match[1].str();
        }
        return {};
    }

    namespace encode_methods
    {
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
            char* pb = (char*)&v.front();
            for (uint32_t i = 0; i < c; i++)
            {
                pb[i] = a[i];
            }
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
            char* pb = (char*)&a.front();
            uint32_t n = d << 2;
            string aa(n, '\0');
            for (uint32_t i = 0; i < n; i++)
            {
                aa[i] = pb[i];
            }
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
    } // namespace encode_methods

#define AUTH_LOGIN_PASSWORD_MD5 "5e543256c480ac577d30f76f9120eb74"
    constexpr char LoginInfoJson[] = "{{\"ip\": \"\", \"acid\": \"1\", \"enc_ver\": \"srun_bx1\", \"username\": \"{}\", \"password\": \"{}\"}}";
    constexpr char ChkSumData[] = "{0}{1}{0}{2}{0}1{0}{0}200{0}1{0}{3}";
    task<map<hstring, hstring>> AuthHelper::LoginDataAsync() const
    {
        using namespace encode_methods;
        string token = co_await ChallengeAsync();
        auto data = map<hstring, hstring>{
            { L"action", L"login" },
            { L"ac_id", L"1" },
            { L"double_stack", L"1" },
            { L"n", L"200" },
            { L"type", L"1" },
            { L"password", L"{MD5}" AUTH_LOGIN_PASSWORD_MD5 }
        };
        string info = "{SRBX1}" + Base64Encode(XEncode(sprint(LoginInfoJson, username, password), token));
        data.emplace(L"info", to_hstring(info));
        data.emplace(L"username", username);
        data.emplace(L"chksum", GetSHA1(to_hstring(sprint(ChkSumData, token, username, AUTH_LOGIN_PASSWORD_MD5, info))));
        return data;
    }

    task<hstring> UseregHelper::LoginAsync() const
    {
        return PostAsync(Uri(LogUri), sprint(LoginData, username, GetMD5(password)));
    }
    task<hstring> UseregHelper::LogoutAsync() const
    {
        return PostAsync(Uri(LogUri), LogoutData);
    }
    task<hstring> UseregHelper::LogoutAsync(wstring const& ip) const
    {
        return PostAsync(Uri(InfoUri), sprint(DropData, ip));
    }

    constexpr wchar_t TableRegex[] = L"<tr align=\"center\">[\\s\\S]+?</tr>";
    constexpr wchar_t ItemRegex[] = L"<td class=\"maintd\">(.*?)</td>";
    task<vector<NetUser>> UseregHelper::UsersAsync() const
    {
        vector<NetUser> result;
        wregex tabler(TableRegex);
        wregex itemr(ItemRegex);
        wstring userhtml(co_await GetAsync(Uri(InfoUri)));
        wsregex_iterator row_begin(userhtml.begin(), userhtml.end(), tabler);
        wsregex_iterator row_end;
        for (; row_begin != row_end; ++row_begin)
        {
            wstring r = row_begin->str();
            wsregex_iterator col_begin(r.begin(), r.end(), itemr);
            wsregex_iterator col_end;
            vector<wstring> details;
            for (; col_begin != col_end; ++col_begin)
            {
                auto& match = *col_begin;
                details.push_back(match[1].str());
            }
            result.push_back({ details[0], details[1], details[10] });
        }
        return result;
    }
} // namespace winrt::TsinghuaNetUWP
