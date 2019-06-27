using System.Threading.Tasks;
using TsinghuaNet.Models;
using Windows.Networking.Connectivity;

namespace TsinghuaNet.Uno.Helpers
{
#if WINDOWS_UWP
    class InternetStatus : NetMapStatus
    {
        private static (NetStatus, string) GetInternetStatus()
        {
            var profile = NetworkInformation.GetInternetConnectionProfile();
            if (profile == null)
                return (NetStatus.Unknown, null);
            var cl = profile.GetNetworkConnectivityLevel();
            if (cl == NetworkConnectivityLevel.None)
                return (NetStatus.Unknown, null);
            if (profile.IsWwanConnectionProfile)
                return (NetStatus.Wwan, null);
            else if (profile.IsWlanConnectionProfile)
                return (NetStatus.Wlan, profile.WlanConnectionProfileDetails.GetConnectedSsid());
            else
                return (NetStatus.Lan, null);
        }

        public override Task RefreshAsync()
        {
            (Status, Ssid) = GetInternetStatus();
            return Task.CompletedTask;
        }
    }
#else
    class InternetStatus : NetPingStatus
    {
    }
#endif
}
