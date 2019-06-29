using System;
using System.Threading.Tasks;
using TsinghuaNet.Models;
using Windows.Networking.Connectivity;

#if __ANDROID__
using Android.App;
using Android.Content;
using Android.Net.Wifi;
#endif

#if __IOS__
using Foundation;
using SystemConfiguration;
#endif

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
#elif __ANDROID__
    class InternetStatus : NetMapStatus
    {
        public override Task RefreshAsync()
        {
            WifiManager wifiManager = (WifiManager)Application.Context.GetSystemService(Context.WifiService);
            if (wifiManager != null)
            {
                Status = NetStatus.Wlan;
                Ssid = wifiManager.ConnectionInfo.SSID;
            }
            else
            {
                Status = NetStatus.Unknown;
                Ssid = string.Empty;
            }
            return Task.CompletedTask;
        }
    }
#elif __IOS__
    class InternetStatus : NetMapStatus
    {
        public override Task RefreshAsync()
        {
            try
            {
                var status = CaptiveNetwork.TryCopyCurrentNetworkInfo("en0", out NSDictionary dict);
                using (dict)
                {
                    if (status == StatusCode.NoKey)
                    {
                        Status = NetStatus.Unknown;
                        Ssid = string.Empty;
                    }
                    Status = NetStatus.Wlan;
                    Ssid = dict[CaptiveNetwork.NetworkInfoKeySSID].ToString();
                }
            }
            catch (Exception)
            {
                Status = NetStatus.Unknown;
                Ssid = string.Empty;
            }
            return Task.CompletedTask;
        }
    }
#else
    class InternetStatus : NetPingStatus
    {
    }
#endif
}
