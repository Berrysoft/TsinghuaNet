#include "pch.h"

#include "CryptographyHelper.h"
#include "UseregHelper.h"
#include <ctime>
#include <deque>
#include <html/html_doc.hpp>
#include <vector>

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
    constexpr wstring_view DetailUri{ L"https://usereg.tsinghua.edu.cn/user_detail_list.php?action=query&start_time={0}-{1}-1&end_time={0}-{1}-{2}&offset={3}" };
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

    IAsyncOperation<IVector<UsageDetail>> UseregHelper::DetailsAsync()
    {
        time_t t = time(nullptr);
        tm now;
        localtime_s(&now, &t);
        html_doc doc{ html_doc::parse(co_await GetBytesAsync(Uri(wsprint(InfoUri, now.tm_year, now.tm_mon, now.tm_mday, 1000)))) };
        deque<UsageDetail> result;
        for (const auto& tr : doc.node()["body"].front()["table"].front()["tr"].front()["td"].back()["table"].back()["tr"])
        {
            if (tr.tag().size() == 3)
            {
                UsageDetail detail;
                sscan(tr[2].front().text().substr(8), "{}", detail.Date);
                char c;
                sscan(tr[5].front().text(), "{}{}", detail.Usage, c);
                switch (c)
                {
                case 'G':
                    detail.Usage *= 1000;
                case 'M':
                    detail.Usage *= 1000;
                case 'K':
                    detail.Usage *= 1000;
                }
                if (result.front().Date == detail.Date)
                {
                    result.front().Usage += detail.Usage;
                }
                else
                {
                    result.push_front(detail);
                }
            }
        }
        co_return single_threaded_vector(vector<UsageDetail>(result.begin(), result.end()));
    }
} // namespace winrt::TsinghuaNetHelper::implementation
