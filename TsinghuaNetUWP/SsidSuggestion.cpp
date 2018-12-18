#include "pch.h"

#include "NetStateBox.h"
#include "SsidSuggestion.h"

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
        for (int s = (int)NetState::Unknown; s <= (int)NetState::Auth6_25; s++)
        {
            m_States.Append(make<NetStateBox>((NetState)s));
        }
    }

    DEPENDENCY_PROPERTY_INIT(Ssid, hstring, SsidSuggestion, box_value(hstring()))
    DEPENDENCY_PROPERTY_INIT(Value, int, SsidSuggestion, box_value(0))
    DEPENDENCY_PROPERTY_INIT(SsidStyle, Windows::UI::Xaml::Style, SsidSuggestion, nullptr)
} // namespace winrt::TsinghuaNetUWP::implementation
