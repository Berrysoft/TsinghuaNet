using System;

namespace TsinghuaNet.Models
{
    public readonly struct FluxUser
    {
        public FluxUser(string username, ByteSize flux, TimeSpan onlineTime, decimal balance)
        {
            Username = username;
            Flux = flux;
            OnlineTime = onlineTime;
            Balance = balance;
        }

        public string Username { get; }

        public ByteSize Flux { get; }

        public TimeSpan OnlineTime { get; }

        public decimal Balance { get; }

        public static bool operator ==(FluxUser u1, FluxUser u2) => u1.Username == u2.Username && u1.Flux == u2.Flux && u1.OnlineTime == u2.OnlineTime && u1.Balance == u2.Balance;
        public static bool operator !=(FluxUser u1, FluxUser u2) => !(u1 == u2);

        public override bool Equals(object obj)
            => obj is FluxUser user && this == user;

        public override int GetHashCode() => Username?.GetHashCode() ?? 0;

        internal static FluxUser Parse(string fluxstr)
        {
            string[] r = fluxstr.Split(',');
            if (string.IsNullOrWhiteSpace(r[0]))
            {
                return default;
            }
            else
            {
                return new FluxUser(
                    r[0],
                    ByteSize.FromBytes(long.Parse(r[6])),
                    TimeSpan.FromSeconds(long.Parse(r[2]) - long.Parse(r[1])),
                    decimal.Parse(r[11]));
            }
        }
    }
}
