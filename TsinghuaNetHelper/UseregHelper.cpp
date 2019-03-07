#include "pch.h"

#include "CryptographyHelper.h"
#include "UseregHelper.h"
#include <linq/to_container.hpp>
#include <regex>

using namespace std;
using namespace sf;
using namespace linq;
using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;

namespace winrt::TsinghuaNetHelper::implementation
{
    constexpr wstring_view LogUri{ L"https://usereg.tsinghua.edu.cn/do.php" };
    constexpr wstring_view InfoUri{ L"https://usereg.tsinghua.edu.cn/online_user_ipv4.php" };
    constexpr wstring_view LoginData{ L"action=login&user_login_name={}&user_password={}" };
    constexpr wstring_view LogoutData{ L"action=logout" };
    constexpr wstring_view DropData{ L"action=drop&user_ip={}" };

    IAsyncOperation<LogResponse> UseregHelper::LoginAsync()
    {
        co_return UserHelper::GetLogResponse(co_await PostAsync(Uri(LogUri), wsprint(LoginData, Username(), GetMD5(Password()))));
    }

    IAsyncOperation<LogResponse> UseregHelper::LogoutAsync()
    {
        co_return UserHelper::GetLogResponse(co_await PostAsync(Uri(LogUri), LogoutData));
    }

    IAsyncOperation<LogResponse> UseregHelper::LogoutAsync(hstring const ip)
    {
        co_return UserHelper::GetLogResponse(co_await PostAsync(Uri(InfoUri), wsprint(DropData, ip)));
    }

    constexpr wchar_t TableRegex[]{ L"<tr align=\"center\">[\\s\\S]+?</tr>" };
    constexpr wchar_t ItemRegex[]{ L"<td class=\"maintd\">(.*?)</td>" };
    IAsyncOperation<IVector<NetUser>> UseregHelper::UsersAsync()
    {
        wregex tabler{ TableRegex };
        wregex itemr{ ItemRegex };
        wstring userhtml(co_await GetAsync(Uri(InfoUri)));
        auto result{
            get_enumerable(wsregex_iterator{ userhtml.begin(), userhtml.end(), tabler }, wsregex_iterator{}) >>
            select([&itemr](auto& m) {
                auto r{ m.str() };
                auto details{
                    get_enumerable(wsregex_iterator{ r.begin(), r.end(), itemr }, wsregex_iterator{}) >>
                    select([](auto& m) {
                        auto& range{ m[1] };
                        return wstring_view(addressof(*range.first), range.length());
                    }) >>
                    to_vector<wstring_view>()
                };
                NetUser t;
                t.Address(hstring(details[0]));
                t.LoginTime(hstring(details[1]));
                t.Client(hstring(details[10]));
                return t;
            })
        };
        co_return single_threaded_vector(result >> to_vector<NetUser>());
    }
} // namespace winrt::TsinghuaNetHelper::implementation
