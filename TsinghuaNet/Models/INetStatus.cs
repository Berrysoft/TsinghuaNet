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
        Task RefreshAsync();
        Task<NetState> SuggestAsync();
    }

    public class NetPingStatus : INetStatus
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
            if (await CanConnectTo("auth4.tsinghua.edu.cn"))
                return NetState.Auth4;
            else if (await CanConnectTo("net.tsinghua.edu.cn"))
                return NetState.Net;
            else if (await CanConnectTo("auth6.tsinghua.edu.cn"))
                return NetState.Auth6;
            else
                return NetState.Unknown;
        }

        public Task RefreshAsync() => Task.CompletedTask;

        public Task<NetState> SuggestAsync() => GetSuggestion();
    }

    public abstract class NetMapStatus : INetStatus
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public NetStatus Status { get; set; }

        public string Ssid { get; set; }

        public abstract Task RefreshAsync();

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
            return Task.Run(() =>
            {
                switch (Status)
                {
                    case NetStatus.Lan:
                        return NetState.Auth4;
                    case NetStatus.Wlan:
                        if (SsidStateMap.ContainsKey(Ssid))
                            return SsidStateMap[Ssid];
                        else
                            return NetState.Unknown;
                    default:
                        return NetState.Unknown;
                }
            });
        }
    }
}
