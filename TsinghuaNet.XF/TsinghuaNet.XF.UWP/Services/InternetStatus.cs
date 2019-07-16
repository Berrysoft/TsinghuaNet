using System.Threading.Tasks;
using TsinghuaNet.Models;
using TsinghuaNet.XF.UWP.Services;
using Windows.Networking.Connectivity;
using Xamarin.Forms;

[assembly: Dependency(typeof(InternetStatus))]

namespace TsinghuaNet.XF.UWP.Services
{
    public class InternetStatus : NetMapStatus
    {
        public override void Refresh()
        {
            var profile = NetworkInformation.GetInternetConnectionProfile();
            if (profile == null)
                Status = NetStatus.Unknown;
            else
            {
                var cl = profile.GetNetworkConnectivityLevel();
                if (cl == NetworkConnectivityLevel.None)
                    Status = NetStatus.Unknown;
                else if (profile.IsWwanConnectionProfile)
                    Status = NetStatus.Wwan;
                else if (profile.IsWlanConnectionProfile)
                {
                    Status = NetStatus.Wlan;
                    Ssid = profile.WlanConnectionProfileDetails.GetConnectedSsid();
                }
                else
                    Status = NetStatus.Lan;
            }
        }
    }
}
