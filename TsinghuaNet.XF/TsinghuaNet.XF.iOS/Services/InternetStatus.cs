using Foundation;
using SystemConfiguration;
using TsinghuaNet.Models;
using TsinghuaNet.XF.iOS.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using System;
using System.Diagnostics;
using System.Linq;

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

        protected override void Refresh()
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
                    if (interfaces.Length == 0)
                    {
                        Status = NetStatus.Unknown;
                    }
                    else
                    {
                        CheckStatus(CaptiveNetwork.TryCopyCurrentNetworkInfo(interfaces[0], out NSDictionary dict));
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
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
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