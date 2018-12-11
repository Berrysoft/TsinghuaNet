#include "pch.h"

#include "UserHelper.h"

using namespace std;
using namespace sf;
using namespace winrt;
using namespace Windows::Foundation;

namespace winrt::TsinghuaNetHelper::implementation
{
    hstring UserHelper::GetFluxString(uint64_t f)
    {
        double flux = (double)f;
        wstring result;
        if (flux < 1000)
        {
            result = sprint(L"{} B", flux);
        }
        else if ((flux /= 1000) < 1000)
        {
            result = sprint(L"{:f2} kB", flux);
        }
        else if ((flux /= 1000) < 1000)
        {
            result = sprint(L"{:f2} MB", flux);
        }
        else
        {
            flux /= 1000;
            result = sprint(L"{:f2} GB", flux);
        }
        return hstring(result);
    }

    hstring UserHelper::GetTimeSpanString(TimeSpan const& time)
    {
        int64_t tsec = time.count() / 10000000;
        bool minus = tsec < 0;
        if (minus)
            tsec = -tsec;
        int64_t h = tsec / 3600;
        tsec -= h * 3600;
        int64_t min = tsec / 60;
        tsec -= min * 60;
        return hstring(sprint(L"{}{:d2}:{:d2}:{:d2}", minus ? L"-" : L"", h, min, tsec));
    }

    hstring UserHelper::GetCurrencyString(double currency)
    {
        return hstring(sprint(L"￥{:f2}", currency));
    }

    hstring UserHelper::GetNetStateString(NetState state)
    {
        switch (state)
        {
        case NetState::Auth4:
            return L"Auth4 - http://auth4.tsinghua.edu.cn/";
        case NetState::Auth6:
            return L"Auth6 - http://auth6.tsinghua.edu.cn/";
        case NetState::Net:
            return L"Net - http://net.tsinghua.edu.cn/";
        case NetState::Auth4_25:
            return L"IPv4 - http://auth4.tsinghua.edu.cn/";
        case NetState::Auth6_25:
            return L"IPv6 - http://auth4.tsinghua.edu.cn/";
        case NetState::Direct:
            return L"不需要登录";
        default:
            return L"未知";
        }
    }

    hstring UserHelper::GetInternetStatusString(InternetStatus status)
    {
        switch (status)
        {
        case InternetStatus::None:
            return L"未连接";
        case InternetStatus::Wwan:
            return L"蜂窝移动网络";
        case InternetStatus::Wlan:
            return L"无线网络";
        case InternetStatus::Lan:
            return L"有线网络";
        default:
            return L"未知";
        }
    }

    constexpr uint64_t BaseFlux = 25000000000;
    uint64_t UserHelper::GetMaxFlux(TsinghuaNetHelper::FluxUser const& user)
    {
        return max(user.Flux(), BaseFlux) + (uint64_t)(user.Balance() / 2 * 1000 * 1000 * 1000);
    }
} // namespace winrt::TsinghuaNetHelper::implementation
