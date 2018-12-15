#include "pch.h"

#include "EditSuggestionDialog.h"

using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml;
using namespace TsinghuaNetHelper;

namespace winrt::TsinghuaNetUWP::implementation
{
    EditSuggestionDialog::EditSuggestionDialog()
    {
        InitializeComponent();
        m_WlanList = single_threaded_observable_vector<IInspectable>();
    }
} // namespace winrt::TsinghuaNetUWP::implementation
