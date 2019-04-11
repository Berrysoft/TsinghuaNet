#include "pch.h"

#include "../Shared/Utility.h"
#include "UserHelper.h"
#include <linq/string.hpp>
#include <linq/to_container.hpp>

using namespace std;
using namespace std::chrono;
using namespace sf;
using namespace linq;
using namespace winrt;
using namespace Windows::Data::Json;
using namespace Windows::Foundation;

namespace winrt::TsinghuaNetHelper::implementation
{
    hstring UserHelper::GetFluxString(uint64_t f)
    {
        double flux{ (double)f };
        wstring result;
        if (flux < 1000)
        {
            result = wsprint(L"{} B", flux);
        }
        else if ((flux /= 1000) < 1000)
        {
            result = wsprint(L"{:f2} kB", flux);
        }
        else if ((flux /= 1000) < 1000)
        {
            result = wsprint(L"{:f2} MB", flux);
        }
        else
        {
            flux /= 1000;
            result = wsprint(L"{:f2} GB", flux);
        }
        return hstring(result);
    }

    hstring UserHelper::GetTimeSpanString(TimeSpan const& time)
    {
        int64_t tsec{ time.count() / 10000000 };
        bool minus{ tsec < 0 };
        if (minus)
            tsec = -tsec;
        int64_t h{ tsec / 3600 };
        tsec -= h * 3600;
        int64_t min{ tsec / 60 };
        tsec -= min * 60;
        return hstring(wsprint(L"{}{:d2}:{:d2}:{:d2}", minus ? L"-" : L"", h, min, tsec));
    }

    hstring UserHelper::GetCurrencyString(double currency)
    {
        return hstring(wsprint(L"￥{:f2}", currency));
    }

    hstring UserHelper::GetNetStateString(NetState state)
    {
        switch (state)
        {
        case NetState::Net:
            return L"Net - http://net.tsinghua.edu.cn/";
        case NetState::Auth4:
            return L"Auth4 - http://auth4.tsinghua.edu.cn/";
        case NetState::Auth6:
            return L"Auth6 - http://auth6.tsinghua.edu.cn/";
        default:
            return L"不需要登录";
        }
    }

    hstring UserHelper::GetInternetStatusString(InternetStatus status)
    {
        switch (status)
        {
        case InternetStatus::Wwan:
            return L"移动流量";
        case InternetStatus::Wlan:
            return L"无线网络";
        case InternetStatus::Lan:
            return L"有线网络";
        default:
            return L"未连接";
        }
    }

    constexpr uint64_t BaseFlux{ 25000000000 };
    uint64_t UserHelper::GetMaxFlux(uint64_t flux, double balance)
    {
        return max(flux, BaseFlux) + (uint64_t)(balance / 2 * 1000 * 1000 * 1000);
    }

    inline unsigned long long stoull(wstring_view str)
    {
        return wcstoull(str.data(), nullptr, 10);
    }

    inline double stod(wstring_view str)
    {
        return wcstod(str.data(), nullptr);
    }

    FluxUser UserHelper::GetFluxUser(hstring const& str)
    {
        auto r{ str >> split(L',') >> to_vector<wstring_view>() };
        FluxUser result{};
        if (!r.empty())
        {
            result.Username = r[0];
            result.Flux = stoull(r[6]);
            result.OnlineTime = seconds(stoull(r[2]) - stoull(r[1]));
            result.Balance = stod(r[11]);
        }
        return result;
    }

    LogResponse UserHelper::GetLogResponse(hstring const& str)
    {
        if (str == L"Login is successful.")
        {
            return { L"登录成功", {}, true };
        }
        else if (str == L"IP has been online, please logout.")
        {
            return { L"已登录" };
        }
        else if (str == L"Logout is successful.")
        {
            return { L"注销成功" };
        }
        else if (str == L"You are not online.")
        {
            return { L"未登录" };
        }
        else
        {
            return { str };
        }
    }

    LogResponse UserHelper::GetAuthLogResponse(hstring const& str, bool login)
    {
        // callback(...)
        JsonObject json{ nullptr };
        if (JsonObject::TryParse(wstring(str.begin() + 9, str.end() - 1), json))
        {
            hstring code{ json.GetNamedString(L"error") };
            hstring msg{ json.GetNamedString(L"error_msg") };
            if (code == L"ok")
            {
                return { login ? L"登录成功" : L"注销成功", {}, true };
            }
            else if (code == L"ip_already_online_error")
            {
                return { L"已登录" };
            }
            else if (code == L"login_error")
            {
                if (msg == L"You are not online.")
                {
                    return { L"未登录" };
                }
                else
                {
                    return { msg };
                }
            }
            else
            {
                auto s{ msg >> split(L':') >> to_vector<wstring_view>() };
                if (!s.empty())
                {
                    if (s.size() == 1)
                    {
                        return { msg };
                    }
                    else
                    {
                        if (s[0] == L"E2616")
                        {
                            s[1] = L"已欠费";
                        }
                        else if (s[0] == L"E2833")
                        {
                            s[1] = L"IP地址异常，请重新拿地址";
                        }
                        return { hstring(s[0]), hstring(s[1]) };
                    }
                }
            }
        }
        return { login ? L"登录失败" : L"注销失败" };
    }

    hstring UserHelper::GetResponseString(LogResponse const& response)
    {
        if (response.Message.empty())
        {
            return response.Code;
        }
        else
        {
            return hstring(wsprint(L"{}：{}", response.Code, response.Message));
        }
    }
} // namespace winrt::TsinghuaNetHelper::implementation
