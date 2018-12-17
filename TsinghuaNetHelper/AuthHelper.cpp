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

    IAsyncOperation<LogResponse> AuthHelper::LoginAsync()
    {
        auto data = co_await LoginDataAsync();
        co_return UserHelper::GetAuthLogResponse(co_await PostMapAsync(Uri(LogUri), data));
    }

    IAsyncOperation<LogResponse> AuthHelper::LogoutAsync()
    {
        auto data = co_await LogoutDataAsync();
        co_return UserHelper::GetAuthLogResponse(co_await PostMapAsync(Uri(LogUri), data));
    }

    IAsyncOperation<FluxUser> AuthHelper::FluxAsync()
    {
        co_return UserHelper::GetFluxUser(co_await PostAsync(Uri(FluxUri)));
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
            return match[1].str();
        }
        return {};
    }

    constexpr char LoginInfoJson[] = "{{\"username\": \"{}\", \"password\": \"{}\", \"ip\": \"\", \"acid\": \"{}\", \"enc_ver\": \"srun_bx1\"}}";
    constexpr char LoginChkSumData[] = "{0}{1}{0}{2}{0}{4}{0}{0}200{0}1{0}{3}";
    IAsyncOperation<IMap<hstring, hstring>> AuthHelper::LoginDataAsync()
    {
        string token = co_await ChallengeAsync();
        hstring md5 = GetHMACMD5(to_hstring(token));
        auto data = single_threaded_map(map<hstring, hstring>{
            { L"action", L"login" },
            { L"ac_id", to_hstring(ac_id) },
            { L"double_stack", L"1" },
            { L"n", L"200" },
            { L"type", L"1" },
            { L"username", Username() },
            { L"password", L"{MD5}" + md5 },
            { L"callback", L"callback" } });
        string info = "{SRBX1}" + Base64Encode(XEncode(sprint(LoginInfoJson, Username(), Password(), ac_id), token));
        data.Insert(L"info", to_hstring(info));
        data.Insert(L"chksum", GetSHA1(to_hstring(sprint(LoginChkSumData, token, Username(), md5, info, ac_id))));
        co_return data;
    }

    constexpr char LogoutInfoJson[] = "{{\"username\": \"{}\", \"ip\": \"\", \"acid\": \"{}\", \"enc_ver\": \"srun_bx1\"}}";
    constexpr char LogoutChkSumData[] = "{0}{1}{0}{3}{0}{0}200{0}1{0}{2}";
    IAsyncOperation<IMap<hstring, hstring>> AuthHelper::LogoutDataAsync()
    {
        string token = co_await ChallengeAsync();
        auto data = single_threaded_map(map<hstring, hstring>{
            { L"action", L"logout" },
            { L"ac_id", to_hstring(ac_id) },
            { L"double_stack", L"1" },
            { L"n", L"200" },
            { L"type", L"1" },
            { L"username", Username() },
            { L"callback", L"callback" } });
        string info = "{SRBX1}" + Base64Encode(XEncode(sprint(LogoutInfoJson, Username(), ac_id), token));
        data.Insert(L"info", to_hstring(info));
        data.Insert(L"chksum", GetSHA1(to_hstring(sprint(LogoutChkSumData, token, Username(), info, ac_id))));
        co_return data;
    }
} // namespace winrt::TsinghuaNetHelper
