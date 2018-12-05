#include "pch.h"

#include "EditSuggestionDialog.h"

using namespace winrt;
using namespace Windows::UI::Xaml;
using namespace TsinghuaNetHelper;

namespace winrt::TsinghuaNetUWP::implementation
{
    EditSuggestionDialog::EditSuggestionDialog()
    {
        InitializeComponent();
        m_States = single_threaded_observable_vector<IInspectable>();
        m_States.Append(box_value(NetState::Unknown));
        m_States.Append(box_value(NetState::Auth4));
        m_States.Append(box_value(NetState::Auth6));
        m_States.Append(box_value(NetState::Net));
        m_States.Append(box_value(NetState::Auth4_25));
        m_States.Append(box_value(NetState::Auth6_25));
        m_States.Append(box_value(NetState::Direct));
    }
} // namespace winrt::TsinghuaNetUWP::implementation
