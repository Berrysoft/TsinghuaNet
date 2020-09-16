using System.Linq;
using Android.App;
using Android.Content;
using Android.Net.Wifi;
using TsinghuaNet.Models;
using TsinghuaNet.XF.Droid.Services;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(InternetStatus))]

namespace TsinghuaNet.XF.Droid.Services
{
    public class InternetStatus : NetMapStatus
    {
        protected override void Refresh()
        {
            var profiles = Connectivity.ConnectionProfiles;
            if (profiles.Contains(ConnectionProfile.WiFi) && Application.Context.GetSystemService(Context.WifiService) is WifiManager wifiManager)
            {
                Status = NetStatus.Wlan;
                Ssid = wifiManager.ConnectionInfo.SSID.Trim('\"');
            }
            else if (profiles.Contains(ConnectionProfile.Cellular))
            {
                Status = NetStatus.Wwan;
            }
            else if (profiles.Contains(ConnectionProfile.Ethernet))
            {
                Status = NetStatus.Lan;
            }
            else
            {
                Status = NetStatus.Unknown;
            }
        }
    }
}