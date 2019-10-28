using System;
using System.Runtime.InteropServices;

namespace TsinghuaNet.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NetCredential
    {
        public IntPtr Username;
        public IntPtr Password;
        public NetState State;
    }
}
