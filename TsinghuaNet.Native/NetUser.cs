using System;
using System.Runtime.InteropServices;

namespace TsinghuaNet.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NetUser
    {
        public long Address;
        public long LoginTime;
        public IntPtr Client;
        public int ClientLength;
    }
}
