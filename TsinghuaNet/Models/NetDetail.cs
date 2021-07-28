using System;

namespace TsinghuaNet.Models
{
    public class NetDetail
    {
        public NetDetail(DateTime login, DateTime logout, ByteSize flux)
        {
            LoginTime = login;
            LogoutTime = logout;
            Flux = flux;
        }

        public DateTime LoginTime { get; }

        public DateTime LogoutTime { get; }

        public ByteSize Flux { get; }

        public static bool operator ==(NetDetail d1, NetDetail d2) => d1.LoginTime == d2.LoginTime && d1.LogoutTime == d2.LogoutTime && d1.Flux == d2.Flux;

        public static bool operator !=(NetDetail d1, NetDetail d2) => !(d1 == d2);

        public override bool Equals(object obj)
            => obj is NetDetail other && this == other;

        public override int GetHashCode() => LoginTime.GetHashCode() ^ LogoutTime.GetHashCode() ^ Flux.GetHashCode();
    }

    public enum NetDetailOrder
    {
        LoginTime,
        LogoutTime,
        Flux
    }
}
