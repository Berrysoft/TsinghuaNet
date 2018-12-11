#include "pch.h"

#include "EditSuggestionDialog.h"
#include "NetStateBox.h"

using namespace winrt;
using namespace Windows::UI::Xaml;
using namespace TsinghuaNetHelper;

namespace winrt::TsinghuaNetUWP::implementation
{
    EditSuggestionDialog::EditSuggestionDialog()
    {
        InitializeComponent();
        m_States = single_threaded_observable_vector<IInspectable>();
        for (int s = (int)NetState::Unknown; s <= (int)NetState::Direct; s++)
        {
            m_States.Append(make<NetStateBox>((NetState)s));
        }
    }
} // namespace winrt::TsinghuaNetUWP::implementation
