﻿#include "pch.h"

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
        m_States = single_threaded_observable_vector<IInspectable>();
        for (int s = (int)NetState::Unknown; s <= (int)NetState::Auth6_25; s++)
        {
            m_States.Append(make<NetStateBox>((NetState)s));
        }
    }

    DEPENDENCY_PROPERTY_INIT(Ssid, hstring, SsidSuggestion, box_value(hstring()))
    DEPENDENCY_PROPERTY_INIT(Value, int, SsidSuggestion, box_value(0))
} // namespace winrt::TsinghuaNetUWP::implementation