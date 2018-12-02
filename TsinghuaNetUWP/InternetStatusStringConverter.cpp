#include "pch.h"

#include "InternetStatusStringConverter.h"
#include <string>

using namespace std;
using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml::Interop;
using namespace TsinghuaNetHelper;

namespace winrt::TsinghuaNetUWP::implementation
{
    IInspectable InternetStatusStringConverter::Convert(IInspectable const& value, TypeName const&, IInspectable const&, hstring const&) const
    {
        InternetStatus status = unbox_value<InternetStatus>(value);
        wstring result;
        switch (status)
        {
        case InternetStatus::None:
            result = L"未连接";
            break;
        case InternetStatus::Wwan:
            result = L"蜂窝移动网络";
            break;
        case InternetStatus::Wlan:
            result = L"无线网络";
            break;
        case InternetStatus::Lan:
            result = L"有线网络";
            break;
        default:
            result = L"未知";
            break;
        }
        return box_value(result);
    }

    IInspectable InternetStatusStringConverter::ConvertBack(IInspectable const&, TypeName const&, IInspectable const&, hstring const&) const
    {
        throw hresult_not_implemented();
    }
} // namespace winrt::TsinghuaNetUWP::implementation
