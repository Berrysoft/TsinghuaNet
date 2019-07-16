using System;
using System.Threading.Tasks;
using Foundation;
using SystemConfiguration;
using TsinghuaNet.Models;
using TsinghuaNet.XF.iOS.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(InternetStatus))]

namespace TsinghuaNet.XF.iOS.Services
{
    public class InternetStatus : NetMapStatus
    {
        public override void Refresh()
        {
            try
            {
                var status = CaptiveNetwork.TryCopyCurrentNetworkInfo("en0", out NSDictionary dict);
                using (dict)
                {
                    if (status == StatusCode.NoKey)
                    {
                        Status = NetStatus.Unknown;
                    }
                    else
                    {
                        Status = NetStatus.Wlan;
                        Ssid = dict[CaptiveNetwork.NetworkInfoKeySSID].ToString();
                    }
                }
            }
            catch (Exception)
            {
                Status = NetStatus.Unknown;
            }
        }
    }
}