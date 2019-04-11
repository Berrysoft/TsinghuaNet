#include "pch.h"

#include "AuthHelper.h"
#include "CryptographyHelper.h"
#include <array>
#include <regex>

using namespace std;
using namespace concurrency;
using namespace sf;
using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;

namespace winrt::TsinghuaNetHelper
{
    constexpr wstring_view LogUriBase{ L"http://auth{}.tsinghua.edu.cn/cgi-bin/srun_portal" };
    constexpr wstring_view FluxUriBase{ L"http://auth{}.tsinghua.edu.cn/rad_user_info.php" };
    constexpr wstring_view ChallengeUriBase{ L"http://auth{}.tsinghua.edu.cn/cgi-bin/get_challenge?username={{}}&double_stack=1&ip&callback=callback" };

    constexpr array acids{ 1, 25, 33, 35 };

    AuthHelper::AuthHelper(int ver, hstring const& username, hstring const& password)
        : NetHelperBase(username, password),
          LogUri(wsprint(LogUriBase, ver)),
          FluxUri(wsprint(FluxUriBase, ver)),
          ChallengeUri(wsprint(ChallengeUriBase, ver))
    {
    }

    IAsyncOperation<LogResponse> AuthHelper::LoginAsync()
    {
        LogResponse response;
        for (int ac_id : acids)
        {
            auto data{ single_threaded_map(co_await LoginDataAsync(ac_id)) };
            response = UserHelper::GetAuthLogResponse(co_await PostAsync(Uri(LogUri), data), true);
            if (response.Succeed)
                break;
        }
        co_return response;
    }

    IAsyncOperation<LogResponse> AuthHelper::LogoutAsync()
    {
        LogResponse response;
        for (int ac_id : acids)
        {
            auto data{ single_threaded_map(co_await LogoutDataAsync(ac_id)) };
            response = UserHelper::GetAuthLogResponse(co_await PostAsync(Uri(LogUri), data), false);
            if (response.Succeed)
                break;
        }
        co_return response;
    }

    IAsyncOperation<FluxUser> AuthHelper::FluxAsync()
    {
        co_return UserHelper::GetFluxUser(co_await PostAsync(Uri(FluxUri)));
    }

    constexpr char ChallengeRegex[]{ "\"challenge\":\"(.*?)\"" };
    task<string> AuthHelper::ChallengeAsync()
    {
        auto result{ co_await GetBytesAsync(Uri(wsprint(ChallengeUri, Username()))) };
        regex reg{ ChallengeRegex };
        smatch match;
        if (regex_search(result, match, reg))
        {
            co_return match[1].str();
        }
        co_return{};
    }

    constexpr char LoginInfoJson[]{ "{{\"username\": \"{}\", \"password\": \"{}\", \"ip\": \"\", \"acid\": \"{}\", \"enc_ver\": \"srun_bx1\"}}" };
    constexpr char LoginChkSumData[]{ "{0}{1}{0}{2}{0}{4}{0}{0}200{0}1{0}{3}" };
    task<map<hstring, hstring>> AuthHelper::LoginDataAsync(int ac_id)
    {
        string token{ co_await ChallengeAsync() };
        hstring md5{ GetHMACMD5(to_hstring(token)) };
        map<hstring, hstring> data{
            { L"action", L"login" },
            { L"ac_id", to_hstring(ac_id) },
            { L"double_stack", L"1" },
            { L"n", L"200" },
            { L"type", L"1" },
            { L"username", Username() },
            { L"password", L"{MD5}" + md5 },
            { L"callback", L"callback" }
        };
        string info{ "{SRBX1}" + Base64Encode(XEncode(sprint(LoginInfoJson, Username(), Password(), ac_id), token)) };
        data.emplace(L"info", to_hstring(info));
        data.emplace(L"chksum", GetSHA1(to_hstring(sprint(LoginChkSumData, token, Username(), md5, info, ac_id))));
        co_return data;
    }

    constexpr char LogoutInfoJson[]{ "{{\"username\": \"{}\", \"ip\": \"\", \"acid\": \"{}\", \"enc_ver\": \"srun_bx1\"}}" };
    constexpr char LogoutChkSumData[]{ "{0}{1}{0}{3}{0}{0}200{0}1{0}{2}" };
    task<map<hstring, hstring>> AuthHelper::LogoutDataAsync(int ac_id)
    {
        string token{ co_await ChallengeAsync() };
        map<hstring, hstring> data{
            { L"action", L"logout" },
            { L"ac_id", to_hstring(ac_id) },
            { L"double_stack", L"1" },
            { L"n", L"200" },
            { L"type", L"1" },
            { L"username", Username() },
            { L"callback", L"callback" }
        };
        string info{ "{SRBX1}" + Base64Encode(XEncode(sprint(LogoutInfoJson, Username(), ac_id), token)) };
        data.emplace(L"info", to_hstring(info));
        data.emplace(L"chksum", GetSHA1(to_hstring(sprint(LogoutChkSumData, token, Username(), info, ac_id))));
        co_return data;
    }
} // namespace winrt::TsinghuaNetHelper
