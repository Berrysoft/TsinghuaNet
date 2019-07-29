using System;
using System.Linq;
using Foundation;
using SystemConfiguration;
using TsinghuaNet.Models;
using TsinghuaNet.XF.iOS.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(InternetStatus))]

namespace TsinghuaNet.XF.iOS.Services
{
    public class InternetStatus : NetMapStatus
    {
        private static void CheckStatus(StatusCode code)
        {
            if (code != StatusCode.OK || code != StatusCode.KeyExists)
            {
                throw new SystemException(code.ToString());
            }
        }

        public override void Refresh()
        {
            var profiles = Connectivity.ConnectionProfiles;
            if (profiles.Contains(ConnectionProfile.Cellular))
            {
                Status = NetStatus.Wwan;
            }
            else if (profiles.Contains(ConnectionProfile.WiFi))
            {
                try
                {
                    CheckStatus(CaptiveNetwork.TryGetSupportedInterfaces(out string[] interfaces));
                    foreach (string name in interfaces)
                    {
                        CheckStatus(CaptiveNetwork.TryCopyCurrentNetworkInfo("en0", out NSDictionary dict));
                        using (dict)
                        {
                            if (dict.TryGetValue(CaptiveNetwork.NetworkInfoKeySSID, out NSObject ssid))
                            {
                                Status = NetStatus.Wlan;
                                Ssid = ssid.ToString();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    Status = NetStatus.Unknown;
                }
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