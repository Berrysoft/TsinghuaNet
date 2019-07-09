using System;
using System.Net;

namespace TsinghuaNet.Models
{
    public struct NetUser
    {
        public NetUser(IPAddress address, DateTime loginTime, string client)
        {
            Address = address;
            LoginTime = loginTime;
            Client = client;
        }

        public IPAddress Address { get; }

        public DateTime LoginTime { get; }

        public string Client { get; }

        public static bool operator ==(NetUser u1, NetUser u2) => u1.Address.Equals(u2.Address) && u1.LoginTime == u2.LoginTime && u1.Client == u2.Client;

        public static bool operator !=(NetUser u1, NetUser u2) => !(u1 == u2);

        public override bool Equals(object obj)
            => obj is NetUser user && this == user;

        public override int GetHashCode() => (Address?.GetHashCode() ?? 0) ^ LoginTime.GetHashCode() ^ (Client?.GetHashCode() ?? 0);
    }
}
