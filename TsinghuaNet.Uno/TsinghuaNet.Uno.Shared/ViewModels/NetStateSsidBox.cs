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
}
