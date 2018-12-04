#include "pch.h"

#include "AuthHelper.h"
#include <pplawait.h>
#include <regex>

using namespace std;
using namespace concurrency;
using namespace sf;
using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;

namespace winrt::TsinghuaNetHelper
{
    constexpr wchar_t LogUriBase[] = L"https://auth{}.tsinghua.edu.cn/cgi-bin/srun_portal";
    constexpr wchar_t FluxUriBase[] = L"https://auth{}.tsinghua.edu.cn/rad_user_info.php";
    constexpr wchar_t ChallengeUriBase[] = L"https://auth{}.tsinghua.edu.cn/cgi-bin/get_challenge?username={{}}&double_stack=1&ip&callback=callback";
    constexpr wchar_t LogoutData[] = L"action=logout";

    AuthHelper::AuthHelper(int ver)
        : LogUri(sprint(LogUriBase, ver)),
          FluxUri(sprint(FluxUriBase, ver)),
          ChallengeUri(sprint(ChallengeUriBase, ver))
    {
    }

    IAsyncOperation<hstring> AuthHelper::LoginAsync()
    {
        auto data = co_await LoginDataAsync();
        auto result = co_await PostMapAsync(Uri(LogUri), data);
        co_return result;
    }

    IAsyncOperation<hstring> AuthHelper::LogoutAsync()
    {
        return PostStringAsync(Uri(LogUri), LogoutData);
    }

    IAsyncOperation<TsinghuaNetHelper::FluxUser> AuthHelper::FluxAsync()
    {
        return TsinghuaNetHelper::FluxUser::Parse(co_await PostAsync(Uri(FluxUri)));
    }

    constexpr char ChallengeRegex[] = "\"challenge\":\"(.*?)\"";
    task<string> AuthHelper::ChallengeAsync()
    {
        auto bytes = co_await GetBytesAsync(Uri(sprint(ChallengeUri, Username())));
        string result((const char*)bytes.data(), bytes.Length());
        regex reg(ChallengeRegex);
        smatch match;
        if (regex_search(result, match, reg))
        {
            co_return match[1].str();
        }
        co_return{};
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
    IAsyncOperation<IMap<hstring, hstring>> AuthHelper::LoginDataAsync()
    {
        using namespace encode_methods;
        string token = co_await ChallengeAsync();
        auto data = single_threaded_map(map<hstring, hstring>{
            { L"action", L"login" },
            { L"ac_id", L"1" },
            { L"double_stack", L"1" },
            { L"n", L"200" },
            { L"type", L"1" },
            { L"password", L"{MD5}" AUTH_LOGIN_PASSWORD_MD5 } });
        string info = "{SRBX1}" + Base64Encode(XEncode(sprint(LoginInfoJson, Username(), Password()), token));
        data.Insert(L"info", to_hstring(info));
        data.Insert(L"username", Username());
        data.Insert(L"chksum", GetSHA1(to_hstring(sprint(ChkSumData, token, Username(), AUTH_LOGIN_PASSWORD_MD5, info))));
        co_return data;
    }
} // namespace winrt::TsinghuaNetHelper
