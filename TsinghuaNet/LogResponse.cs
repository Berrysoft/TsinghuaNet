using System;
using Newtonsoft.Json.Linq;

namespace TsinghuaNet
{
    /// <summary>
    /// The response of Login or Logout.
    /// </summary>
    public struct LogResponse
    {
        /// <summary>
        /// Initialize a new instance of <see cref="LogResponse"/> class.
        /// </summary>
        /// <param name="succeed">Whether the command is succeed.</param>
        /// <param name="message">The formatted response message.</param>
        public LogResponse(bool succeed, string message)
        {
            Succeed = succeed;
            Message = message;
        }

        /// <summary>
        /// Shows whether the command is succeed.
        /// </summary>
        public bool Succeed { get; }
        /// <summary>
        /// The formatted response message.
        /// </summary>
        public string Message { get; }

        public static bool operator ==(LogResponse r1, LogResponse r2) => r1.Succeed == r2.Succeed && r1.Message == r2.Message;
        public static bool operator !=(LogResponse r1, LogResponse r2) => !(r1 == r2);

        /// <summary>
        /// Determines whether the two <see cref="LogResponse"/> are equal.
        /// </summary>
        /// <param name="obj">The other object.</param>
        /// <returns><see langword="true"/> if they're equal; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is LogResponse other)
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
            return Succeed.GetHashCode() ^ (Message?.GetHashCode() ?? 0);
        }

        internal static LogResponse ParseFromNet(string response)
        {
            return new LogResponse(response == "Login is successful.", response);
        }

        internal static LogResponse ParseFromAuth(string response)
        {
            try
            {
                string jsonstr = response.Substring(9, response.Length - 10);
                JObject json = JObject.Parse(jsonstr);
                return new LogResponse((string)json["error"] == "ok", $"error: {json["error"]}; error_msg: {json["error_msg"]}");
            }
            catch (Exception)
            {
                return new LogResponse(false, response);
            }
        }

        internal static LogResponse ParseFromUsereg(string response)
        {
            return new LogResponse(response == "ok", response);
        }
    }
}
