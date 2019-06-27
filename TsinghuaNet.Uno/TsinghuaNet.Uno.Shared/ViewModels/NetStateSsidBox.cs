using System.Collections.Generic;
using TsinghuaNet.Helpers;

namespace TsinghuaNet.Uno.ViewModels
{
    sealed class NetStateSsidBox
    {
        public NetStateSsidBox(string ssid, NetState value)
        {
            Ssid = ssid;
            Value = value;
        }

        public string Ssid { get; set; }
        public NetState Value { get; set; }
    }

    static class NetStateHelper
    {
        public readonly static List<string> NetStateNameList = new List<string>() { "不登录", "Net", "Auth4", "Auth6" };
    }
}
