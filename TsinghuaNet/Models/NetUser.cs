using System;
using System.Net;
using System.Net.NetworkInformation;

namespace TsinghuaNet.Models
{
    public struct NetUser
    {
        public NetUser(IPAddress address, DateTime loginTime, PhysicalAddress mac)
        {
            Address = address;
            LoginTime = loginTime;
            MacAddress = mac;
        }

        public IPAddress Address { get; }

        public DateTime LoginTime { get; }

        public PhysicalAddress MacAddress { get; }

        public static bool operator ==(NetUser u1, NetUser u2) => u1.Address.Equals(u2.Address) && u1.LoginTime == u2.LoginTime && u1.MacAddress.Equals(u2.MacAddress);

        public static bool operator !=(NetUser u1, NetUser u2) => !(u1 == u2);

        public override bool Equals(object obj)
            => obj is NetUser user && this == user;

        public override int GetHashCode() => (Address?.GetHashCode() ?? 0) ^ LoginTime.GetHashCode() ^ (MacAddress?.GetHashCode() ?? 0);
    }
}
