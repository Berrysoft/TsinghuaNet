#include "pch.h"

#include "CryptographyHelper.h"
#include "NetHelper.h"

using namespace std;
using sf::sprint;
using namespace winrt;
using namespace Windows::Foundation;

namespace winrt::TsinghuaNetHelper::implementation
{
    constexpr wstring_view LogUri{ L"http://net.tsinghua.edu.cn/do_login.php" };
    constexpr wstring_view FluxUri{ L"http://net.tsinghua.edu.cn/rad_user_info.php" };
    constexpr wstring_view LoginData{ L"action=login&ac_id=1&username={}&password={{MD5_HEX}}{}" };
    constexpr wstring_view LogoutData{ L"action=logout" };

    IAsyncOperation<LogResponse> NetHelper::LoginAsync()
    {
        co_return UserHelper::GetLogResponse(co_await PostAsync(Uri(LogUri), sprint(LoginData, Username(), GetMD5(Password()))));
    }

    IAsyncOperation<LogResponse> NetHelper::LogoutAsync()
    {
        co_return UserHelper::GetLogResponse(co_await PostAsync(Uri(LogUri), LogoutData));
    }

    IAsyncOperation<FluxUser> NetHelper::FluxAsync()
    {
        co_return UserHelper::GetFluxUser(co_await PostAsync(Uri(FluxUri)));
    }
} // namespace winrt::TsinghuaNetHelper::implementation
