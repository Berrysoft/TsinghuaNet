#include "pch.h"

#include "AuthHelper.h"
#include "CryptographyHelper.h"
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
    constexpr wchar_t LogUriBase[] = L"http://auth{}.tsinghua.edu.cn/cgi-bin/srun_portal";
    constexpr wchar_t FluxUriBase[] = L"http://auth{}.tsinghua.edu.cn/rad_user_info.php";
    constexpr wchar_t ChallengeUriBase[] = L"http://auth{}.tsinghua.edu.cn/cgi-bin/get_challenge?username={{}}&double_stack=1&ip&callback=callback";

    AuthHelper::AuthHelper(int ver, int ac_id)
        : LogUri(sprint(LogUriBase, ver)),
          FluxUri(sprint(FluxUriBase, ver)),
          ChallengeUri(sprint(ChallengeUriBase, ver)),
          ac_id(ac_id)
    {
    }

    IAsyncOperation<hstring> AuthHelper::LoginAsync()
    {
        auto data = co_await LoginDataAsync();
        co_return co_await PostMapAsync(Uri(LogUri), data);
    }

    IAsyncOperation<hstring> AuthHelper::LogoutAsync()
    {
        auto data = co_await LogoutDataAsync();
        co_return co_await PostMapAsync(Uri(LogUri), data);
    }

    IAsyncOperation<FluxUser> AuthHelper::FluxAsync()
    {
        return FluxUser::Parse(co_await PostAsync(Uri(FluxUri)));
    }

    constexpr char ChallengeRegex[] = "\"(.*?)\":\"(.*?)\"";
    task<map<string, string>> AuthHelper::ChallengeAsync()
    {
        auto bytes = co_await GetBytesAsync(Uri(sprint(ChallengeUri, Username())));
        string result((const char*)bytes.data(), bytes.Length());
        map<string, string> m;
        regex reg(ChallengeRegex);
        sregex_iterator beginr(result.begin(), result.end(), reg);
        sregex_iterator endr;
        for (; beginr != endr; ++beginr)
        {
            auto match = *beginr;
            m.emplace(match[1].str(), match[2].str());
        }
        co_return m;
    }

    constexpr char LoginInfoJson[] = "{{\"username\": \"{}\", \"password\": \"{}\", \"ip\": \"\", \"acid\": \"{}\", \"enc_ver\": \"srun_bx1\"}}";
    constexpr char LoginChkSumData[] = "{0}{1}{0}{2}{0}{4}{0}{0}200{0}1{0}{3}";
    IAsyncOperation<IMap<hstring, hstring>> AuthHelper::LoginDataAsync()
    {
        map<string, string> challenge = co_await ChallengeAsync();
        string token = challenge["challenge"];
        string pwd = challenge["password"];
        if (pwd.empty())
            pwd = "undefined";
        hstring md5 = GetHMACMD5(token, pwd);
        auto data = single_threaded_map(map<hstring, hstring>{
            { L"action", L"login" },
            { L"ac_id", to_hstring(ac_id) },
            { L"double_stack", L"1" },
            { L"n", L"200" },
            { L"type", L"1" },
            { L"username", Username() },
            { L"password", L"{MD5}" + md5 } });
        string info = "{SRBX1}" + Base64Encode(XEncode(sprint(LoginInfoJson, Username(), Password(), ac_id), token));
        data.Insert(L"info", to_hstring(info));
        data.Insert(L"chksum", GetSHA1(to_hstring(sprint(LoginChkSumData, token, Username(), md5, info, ac_id))));
        co_return data;
    }

    constexpr char LogoutInfoJson[] = "{{\"ip\": \"\", \"acid\": \"{}\", \"enc_ver\": \"srun_bx1\", \"username\": \"{}\"}}";
    constexpr char LogoutChkSumData[] = "{0}{1}{0}{3}{0}{0}200{0}1{0}{2}";
    IAsyncOperation<IMap<hstring, hstring>> AuthHelper::LogoutDataAsync()
    {
        map<string, string> challenge = co_await ChallengeAsync();
        string token = challenge["challenge"];
        auto data = single_threaded_map(map<hstring, hstring>{
            { L"action", L"logout" },
            { L"ac_id", to_hstring(ac_id) },
            { L"double_stack", L"1" },
            { L"n", L"200" },
            { L"type", L"1" },
            { L"username", Username() } });
        string info = "{SRBX1}" + Base64Encode(XEncode(sprint(LogoutInfoJson, ac_id, Username()), token));
        data.Insert(L"info", to_hstring(info));
        data.Insert(L"chksum", GetSHA1(to_hstring(sprint(LogoutChkSumData, token, Username(), info, ac_id))));
        co_return data;
    }
} // namespace winrt::TsinghuaNetHelper
