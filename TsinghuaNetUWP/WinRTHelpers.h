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

    namespace TsinghuaNetUWP
    {
        struct UserContentManager
        {
            UserContentManager(IUserContent const& content) : content(content) { content.IsProgressActive(true); }
            ~UserContentManager() { content.IsProgressActive(false); }
        private:
            IUserContent content;
        };
    } // namespace TsinghuaNetUWP
} // namespace winrt
