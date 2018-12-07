#include "pch.h"

#include "UseregHelper.h"
#include <regex>

using namespace std;
using namespace sf;
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

    IAsyncOperation<hstring> UseregHelper::LoginAsync()
    {
        return base.PostStringAsync(Uri(LogUri), hstring(sprint(LoginData, base.Username(), GetMD5(base.Password()))));
    }

    IAsyncOperation<hstring> UseregHelper::LogoutAsync()
    {
        return base.PostStringAsync(Uri(LogUri), LogoutData);
    }

    IAsyncOperation<hstring> UseregHelper::LogoutAsync(hstring const ip)
    {
        return base.PostStringAsync(Uri(InfoUri), hstring(sprint(DropData, ip)));
    }

    constexpr wchar_t TableRegex[] = L"<tr align=\"center\">[\\s\\S]+?</tr>";
    constexpr wchar_t ItemRegex[] = L"<td class=\"maintd\">(.*?)</td>";
    IAsyncOperation<IVector<NetUser>> UseregHelper::UsersAsync()
    {
        auto result = single_threaded_vector<NetUser>();
        wregex tabler(TableRegex);
        wregex itemr(ItemRegex);
        wstring userhtml(co_await base.GetAsync(Uri(InfoUri)));
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
            NetUser t;
            t.Address(details[0]);
            t.LoginTime(details[1]);
            t.Client(details[10]);
            result.Append(t);
        }
        return result;
    }
} // namespace winrt::TsinghuaNetHelper::implementation
