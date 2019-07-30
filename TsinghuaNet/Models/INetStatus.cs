using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using PropertyChanged;

namespace TsinghuaNet.Models
{
    public enum NetStatus
    {
        Unknown,
        Wwan,
        Wlan,
        Lan
    }

    public interface INetStatus : INotifyPropertyChanged
    {
        NetStatus Status { get; set; }
        string Ssid { get; set; }
        void Refresh();
        Task<NetState> SuggestAsync();
    }

    public class NetPingStatus : INetStatus
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        [DoNotNotify]
        public NetStatus Status { get; set; }

        [DoNotNotify]
        public string Ssid { get; set; }

        private static async Task<bool> CanConnectTo(string uri)
        {
            try
            {
                using (Ping p = new Ping())
                {
                    var reply = await p.SendPingAsync(uri);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static async Task<NetState> GetSuggestion()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                if (await CanConnectTo("auth4.tsinghua.edu.cn"))
                    return NetState.Auth4;
                else if (await CanConnectTo("net.tsinghua.edu.cn"))
                    return NetState.Net;
                else if (await CanConnectTo("auth6.tsinghua.edu.cn"))
                    return NetState.Auth6;
            }
            return NetState.Unknown;
        }

        public void Refresh() { }

        public Task<NetState> SuggestAsync() => GetSuggestion();
    }

    public abstract class NetMapStatus : INetStatus
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        public NetStatus Status { get; set; }

        public string Ssid { get; set; }

        public abstract void Refresh();

        private static readonly Dictionary<string, NetState> SsidStateMap = new Dictionary<string, NetState>()
        {
            ["Tsinghua"] = NetState.Net,
            ["Tsinghua-5G"] = NetState.Net,
            ["Tsinghua-IPv4"] = NetState.Auth4,
            ["Tsinghua-IPv6"] = NetState.Auth6,
            ["Wifi.郑裕彤讲堂"] = NetState.Net
        };

        public Task<NetState> SuggestAsync()
        {
            return Task.FromResult(Status switch
            {
                NetStatus.Lan => NetState.Auth4,
                NetStatus.Wlan => SsidStateMap.ContainsKey(Ssid) ? SsidStateMap[Ssid] : NetState.Unknown,
                _ => NetState.Unknown
            });
        }
    }
}
