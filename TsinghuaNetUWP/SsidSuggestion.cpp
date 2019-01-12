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
        m_States.Append(make<NetStateBox>(NetState::Auth4_25));
    }

    int SsidSuggestion::ValueToIndex(NetState value) const
    {
        auto i{ m_States >>
                select([](auto s) { return s.try_as<TsinghuaNetUWP::NetStateBox>(); }) >>
                index_of([=](auto box) { return box.Value() == value; }) };
        return i != size_t(-1) ? (int)i : 0;
    }

    NetState SsidSuggestion::IndexToValue(int index) const
    {
        return m_States.GetAt(index).try_as<TsinghuaNetUWP::NetStateBox>().Value();
    }

    DEPENDENCY_PROPERTY_INIT(Ssid, hstring, SsidSuggestion, box_value(hstring{}))
    DEPENDENCY_PROPERTY_INIT(Value, int, SsidSuggestion, box_value(0))
    DEPENDENCY_PROPERTY_INIT(SsidStyle, Windows::UI::Xaml::Style, SsidSuggestion, nullptr)
} // namespace winrt::TsinghuaNetUWP::implementation
