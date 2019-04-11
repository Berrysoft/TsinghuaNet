#include "pch.h"

#include "NetStateBox.h"
#include "SsidSuggestion.h"

using namespace linq;
using namespace winrt;
using namespace Windows::UI::Xaml;
using namespace TsinghuaNetHelper;

namespace winrt::TsinghuaNetUWP::implementation
{
    SsidSuggestion::SsidSuggestion()
    {
        InitializeComponent();
        // 向列表中添加可选项
        m_States = single_threaded_observable_vector<IInspectable>();
        m_States.Append(make<NetStateBox>(NetState::Unknown));
        m_States.Append(make<NetStateBox>(NetState::Net));
        m_States.Append(make<NetStateBox>(NetState::Auth4));
    }

    DEPENDENCY_PROPERTY_INIT(Ssid, hstring, SsidSuggestion, box_value(hstring{}))
    DEPENDENCY_PROPERTY_INIT(Value, NetState, SsidSuggestion, box_value(NetState::Unknown))
    DEPENDENCY_PROPERTY_INIT(SsidStyle, Windows::UI::Xaml::Style, SsidSuggestion, nullptr)
} // namespace winrt::TsinghuaNetUWP::implementation
