#include "pch.h"

#include "EditSuggestionDialog.h"

using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml;

namespace winrt::TsinghuaNetUWP::implementation
{
    EditSuggestionDialog::EditSuggestionDialog()
    {
        InitializeComponent();
        // 初始化列表，添加内容由MainPage完成
        m_WlanList = single_threaded_observable_vector<IInspectable>();
    }

    /// <summary>
    /// 点击加号弹出对话框
    /// </summary>
    /// <param name="e"></param>
    void EditSuggestionDialog::AddSelection(IInspectable const&, RoutedEventArgs const& e)
    {
        // 如果直接设置Flyout属性会给AppBarButton添加一个难看的箭头
        AddFlyout().ShowAt(e.OriginalSource().as<FrameworkElement>());
    }

    /// <summary>
    /// 点击添加按钮添加SSID
    /// </summary>
    void EditSuggestionDialog::AddButtonClick(IInspectable const&, RoutedEventArgs const&)
    {
        hstring ssid = AddFlyoutText().Text();
        if (!ssid.empty()) // SSID不能为空
        {
            AddFlyout().Hide();
            // 清空文本框
            AddFlyoutText().Text(hstring());
            // 如果列表中已存在，不添加
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

    /// <summary>
    /// 删除选定项
    /// </summary>
    void EditSuggestionDialog::DeleteSelection(IInspectable const&, RoutedEventArgs const&)
    {
        m_WlanList.RemoveAt(WlanListView().SelectedIndex());
    }

    /// <summary>
    /// 点击问号弹出对话框
    /// </summary>
    /// <param name="e"></param>
    void EditSuggestionDialog::HelpSelection(IInspectable const&, RoutedEventArgs const& e)
    {
        HelpFlyout().ShowAt(e.OriginalSource().as<FrameworkElement>());
    }
} // namespace winrt::TsinghuaNetUWP::implementation
