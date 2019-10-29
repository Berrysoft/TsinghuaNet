using System.Runtime.InteropServices;

namespace TsinghuaNet.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NetDetail
    {
        public long LoginTime;
        public long LogoutTime;
        public long Flux;
    }
}
