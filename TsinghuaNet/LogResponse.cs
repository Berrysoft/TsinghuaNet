using System;
using System.Text.Json;

namespace TsinghuaNet
{
    public readonly struct LogResponse
    {
        public LogResponse(bool succeed, string message)
        {
            Succeed = succeed;
            Message = message;
        }

        public readonly bool Succeed;

        public readonly string Message;

        public static bool operator ==(LogResponse r1, LogResponse r2) => r1.Succeed == r2.Succeed && r1.Message == r2.Message;
        public static bool operator !=(LogResponse r1, LogResponse r2) => !(r1 == r2);

        public override bool Equals(object obj)
            => obj is LogResponse res && this == res;

        public override int GetHashCode() => Succeed.GetHashCode() ^ (Message?.GetHashCode() ?? 0);

        internal static LogResponse ParseFromNet(string response)
        {
            return new LogResponse(response == "Login is successful.", response);
        }

        internal static LogResponse ParseFromAuth(string response)
        {
            try
            {
                string jsonstr = response.Substring(9, response.Length - 10);
                JsonDocument json = JsonDocument.Parse(jsonstr);
                JsonElement root = json.RootElement;
                string error = root.GetProperty("error").GetString();
                string error_msg = root.GetProperty("error_msg").GetString();
                return new LogResponse(error == "ok", $"error: {error}; error_msg: {error_msg}");
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
