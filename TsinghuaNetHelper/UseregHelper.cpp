#include "pch.h"

#include "CryptographyHelper.h"
#include "UseregHelper.h"
#include <regex>

using namespace std;
using sf::sprint;
using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;

namespace winrt::TsinghuaNetHelper::implementation
{
    constexpr wchar_t LogUri[] = L"https://usereg.tsinghua.edu.cn/do.php";
    constexpr wchar_t InfoUri[] = L"https://usereg.tsinghua.edu.cn/online_user_ipv4.php";
    constexpr wchar_t LoginData[] = L"action=login&user_login_name={}&user_password={}";
    constexpr wchar_t LogoutData[] = L"action=logout";
    constexpr wchar_t DropData[] = L"action=drop&user_ip={}";

    IAsyncOperation<LogResponse> UseregHelper::LoginAsync()
    {
        co_return UserHelper::GetLogResponse(co_await PostAsync(Uri(LogUri), hstring(sprint(LoginData, Username(), GetMD5(Password())))));
    }

    IAsyncOperation<LogResponse> UseregHelper::LogoutAsync()
    {
        co_return UserHelper::GetLogResponse(co_await PostAsync(Uri(LogUri), LogoutData));
    }

    IAsyncOperation<LogResponse> UseregHelper::LogoutAsync(hstring const ip)
    {
        co_return UserHelper::GetLogResponse(co_await PostAsync(Uri(InfoUri), hstring(sprint(DropData, ip))));
    }

    constexpr wchar_t TableRegex[] = L"<tr align=\"center\">[\\s\\S]+?</tr>";
    constexpr wchar_t ItemRegex[] = L"<td class=\"maintd\">(.*?)</td>";
    IAsyncOperation<IVector<NetUser>> UseregHelper::UsersAsync()
    {
        auto result = single_threaded_vector<NetUser>();
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
            vector<wstring_view> details;
            for (; col_begin != col_end; ++col_begin)
            {
                auto& match = *col_begin;
                auto& range = match[1];
                details.emplace_back(addressof(*range.first), range.length());
            }
            NetUser t;
            t.Address(hstring(details[0]));
            t.LoginTime(hstring(details[1]));
            t.Client(hstring(details[10]));
            result.Append(t);
        }
        return result;
    }
} // namespace winrt::TsinghuaNetHelper::implementation
