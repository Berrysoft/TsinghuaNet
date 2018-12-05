#include "pch.h"

#include "NetHelper.h"

using namespace sf;
using namespace winrt;
using namespace Windows::Foundation;

namespace winrt::TsinghuaNetHelper::implementation
{
    constexpr wchar_t LogUri[] = L"http://net.tsinghua.edu.cn/do_login.php";
    constexpr wchar_t FluxUri[] = L"http://net.tsinghua.edu.cn/rad_user_info.php";
    constexpr wchar_t LoginData[] = L"action=login&ac_id=1&username={}&password={{MD5_HEX}}{}";
    constexpr wchar_t LogoutData[] = L"action=logout";

    IAsyncOperation<hstring> NetHelper::LoginAsync()
    {
        return base.PostStringAsync(Uri(LogUri), hstring(sprint(LoginData, base.Username(), GetMD5(base.Password()))));
    }

    IAsyncOperation<hstring> NetHelper::LogoutAsync()
    {
        return base.PostStringAsync(Uri(LogUri), LogoutData);
    }

    IAsyncOperation<TsinghuaNetHelper::FluxUser> NetHelper::FluxAsync()
    {
        return TsinghuaNetHelper::FluxUser::Parse(co_await base.PostAsync(Uri(FluxUri)));
    }
} // namespace winrt::TsinghuaNetHelper::implementation
