#pragma once

namespace winrt
{
    namespace Windows::Networking::Connectivity
    {
        struct NetworkListener
        {
            NetworkListener() { m_Token = NetworkInformation::NetworkStatusChanged({ this, &NetworkListener::OnNetworkStatusChanged }); }
            ~NetworkListener() { NetworkInformation::NetworkStatusChanged(m_Token); }

            event_token NetworkStatusChanged(NetworkStatusChangedEventHandler const& handler) { return m_NetworkStatusChanged.add(handler); }
            void NetworkStatusChanged(event_token const& token) { m_NetworkStatusChanged.remove(token); }

        private:
            event_token m_Token;
            event<NetworkStatusChangedEventHandler> m_NetworkStatusChanged;

            void OnNetworkStatusChanged(Windows::Foundation::IInspectable const& sender) { m_NetworkStatusChanged(sender); }
        };
    } // namespace Windows::Networking::Connectivity

    namespace Windows::UI::Xaml::Controls
    {
        /// <summary>
        /// 一个帮助类，管理<see cref="Windows.UI.Xaml.Controls.ProgressRing"/>的活动状态
        /// </summary>
        struct ProgressRingManager
        {
            ProgressRingManager(ProgressRing const& ring) : ring(ring) { ring.IsActive(true); }
            ~ProgressRingManager() { ring.IsActive(false); }

        private:
            ProgressRing ring;
        };
    } // namespace Windows::UI::Xaml::Controls
} // namespace winrt
