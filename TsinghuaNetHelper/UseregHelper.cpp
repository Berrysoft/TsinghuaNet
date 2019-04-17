#include "pch.h"

#include "CryptographyHelper.h"
#include "UseregHelper.h"
#include <html/html_doc.hpp>

using namespace std;
using namespace sf;
using namespace html;
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

    IAsyncOperation<IVector<NetUser>> UseregHelper::UsersAsync()
    {
        html_doc doc{ html_doc::parse(co_await GetBytesAsync(Uri(InfoUri))) };
        vector<NetUser> result;
        for (const auto& tr : (*(++doc.node()["body"].front()["table"].front()["tr"].front()["td"].back()["table"].begin()))["tr"])
        {
            if (tr.tag().size() == 1)
            {
                NetUser user;
                user.Address(to_hstring(tr[1].front().text()));
                user.LoginTime(to_hstring(tr[2].front().text()));
                user.Client(to_hstring(tr[11].front().text()));
                result.push_back(user);
            }
        }
        co_return single_threaded_vector(move(result));
    }
} // namespace winrt::TsinghuaNetHelper::implementation
