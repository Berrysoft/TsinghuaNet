#include "pch.h"

#include "NetStateStringConverter.h"
#include <string>

using namespace std;
using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml::Interop;
using namespace TsinghuaNetHelper;

namespace winrt::TsinghuaNetUWP::implementation
{
    IInspectable NetStateStringConverter::Convert(IInspectable const& value, TypeName const&, IInspectable const&, hstring const&) const
    {
        NetState state = unbox_value<NetState>(value);
        wstring result;
        switch (state)
        {
        case NetState::Auth4:
            result = L"Auth4 - https://auth4.tsinghua.edu.cn/";
            break;
        case NetState::Auth6:
            result = L"Auth6 - https://auth6.tsinghua.edu.cn/";
            break;
        case NetState::Net:
            result = L"Net - http://net.tsinghua.edu.cn/";
            break;
        case NetState::Direct:
            result = L"不需要登录";
            break;
        default:
            result = L"未知";
            break;
        }
        return box_value(result);
    }

    IInspectable NetStateStringConverter::ConvertBack(IInspectable const&, TypeName const&, IInspectable const&, hstring const&) const
    {
        throw hresult_not_implemented();
    }
} // namespace winrt::TsinghuaNetUWP::implementation
