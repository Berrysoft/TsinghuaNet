using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Net.Wifi;
using TsinghuaNet.Models;
using TsinghuaNet.XF.Droid.Services;

[assembly: Xamarin.Forms.Dependency(typeof(InternetStatus))]

namespace TsinghuaNet.XF.Droid.Services
{
    public class InternetStatus : NetMapStatus
    {
        public override void Refresh()
        {
            WifiManager wifiManager = (WifiManager)Application.Context.GetSystemService(Context.WifiService);
            if (wifiManager != null)
            {
                Status = NetStatus.Wlan;
                Ssid = wifiManager.ConnectionInfo.SSID.Trim('\"');
            }
            else
            {
                Status = NetStatus.Unknown;
            }
        }
    }
}