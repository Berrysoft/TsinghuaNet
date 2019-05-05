using System;

namespace TsinghuaNet
{
    /// <summary>
    /// A simple structure represents the current user online.
    /// </summary>
    public readonly struct FluxUser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FluxUser"/> class.
        /// </summary>
        /// <param name="username">Username of the user.</param>
        /// <param name="flux">Flux used by the user this month.</param>
        /// <param name="onlineTime">Online time used this time of the user.</param>
        /// <param name="balance">The network balance of the user.</param>
        public FluxUser(string username, long flux, TimeSpan onlineTime, decimal balance)
        {
            Username = username;
            Flux = flux;
            OnlineTime = onlineTime;
            Balance = balance;
        }
        /// <summary>
        /// Username of the user.
        /// </summary>
        public string Username { get; }
        /// <summary>
        /// Flux used by the user this month.
        /// </summary>
        public long Flux { get; }
        /// <summary>
        /// Online time used this time of the user.
        /// </summary>
        public TimeSpan OnlineTime { get; }
        /// <summary>
        /// The network balance of the user.
        /// </summary>
        public decimal Balance { get; }

        public static bool operator ==(FluxUser u1, FluxUser u2) => u1.Username == u2.Username;
        public static bool operator !=(FluxUser u1, FluxUser u2) => !(u1 == u2);

        /// <summary>
        /// Determines whether the username of the two <see cref="FluxUser"/> are equal.
        /// </summary>
        /// <param name="obj">The other object.</param>
        /// <returns><see langword="true"/> if they're equal; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is FluxUser other)
            {
                return this == other;
            }
            return false;
        }
        /// <summary>
        /// Returns the hash value of this object.
        /// </summary>
        /// <returns>The hash value.</returns>
        public override int GetHashCode()
        {
            return Username?.GetHashCode() ?? 0;
        }

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
                    long.Parse(r[6]),
                    TimeSpan.FromSeconds(long.Parse(r[2]) - long.Parse(r[1])),
                    decimal.Parse(r[11]));
            }
        }
    }
}
