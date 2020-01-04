using System.Runtime.InteropServices;

namespace TsinghuaNet.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct NetUser
    {
        public long Address;
        public long LoginTime;
        public fixed byte MacAddress[6];
    }
}
