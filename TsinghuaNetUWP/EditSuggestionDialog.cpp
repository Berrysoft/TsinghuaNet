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

    void EditSuggestionDialog::AddSelection(IInspectable const&, RoutedEventArgs const& e)
    {
        AddFlyout().ShowAt(e.OriginalSource().as<FrameworkElement>());
    }

    void EditSuggestionDialog::AddButtonClick(IInspectable const&, RoutedEventArgs const&)
    {
        hstring ssid = AddFlyoutText().Text();
        if (!ssid.empty())
        {
            AddFlyout().Hide();
            AddFlyoutText().Text(hstring());
            for (auto pair : m_WlanList)
            {
                auto item = pair.try_as<TsinghuaNetUWP::NetStateSsidBox>();
                if (item && item.Ssid() == ssid)
                {
                    return;
                }
            }
            auto item = make<NetStateSsidBox>();
            item.Ssid(ssid);
            m_WlanList.Append(item);
        }
    }

    void EditSuggestionDialog::DeleteSelection(IInspectable const&, RoutedEventArgs const&)
    {
        m_WlanList.RemoveAt(WlanListView().SelectedIndex());
    }
} // namespace winrt::TsinghuaNetUWP::implementation
