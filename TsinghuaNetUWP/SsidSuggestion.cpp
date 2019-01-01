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
        m_States.Append(make<NetStateBox>(NetState::Unknown));
        m_States.Append(make<NetStateBox>(NetState::Net));
        m_States.Append(make<NetStateBox>(NetState::Auth4));
        m_States.Append(make<NetStateBox>(NetState::Auth4_25));
    }

    int SsidSuggestion::ValueToIndex(NetState value) const
    {
        int size{ (int)m_States.Size() };
        for (int i{ 0 }; i < size; i++)
        {
            if (m_States.GetAt(i).try_as<TsinghuaNetUWP::NetStateBox>().Value() == value)
            {
                return i;
            }
        }
        return 0;
    }

    NetState SsidSuggestion::IndexToValue(int index) const
    {
        return m_States.GetAt(index).try_as<TsinghuaNetUWP::NetStateBox>().Value();
    }

    DEPENDENCY_PROPERTY_INIT(Ssid, hstring, SsidSuggestion, box_value(hstring{}))
    DEPENDENCY_PROPERTY_INIT(Value, int, SsidSuggestion, box_value(0))
    DEPENDENCY_PROPERTY_INIT(SsidStyle, Windows::UI::Xaml::Style, SsidSuggestion, nullptr)
} // namespace winrt::TsinghuaNetUWP::implementation
