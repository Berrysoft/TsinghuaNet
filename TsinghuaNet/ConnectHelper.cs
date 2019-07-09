using System.Net.Http;

namespace TsinghuaNet
{
    public enum NetState
    {
        Unknown,
        Net,
        Auth4,
        Auth6
    }

    public static class ConnectHelper
    {
        public static IConnect GetHelper(NetState state) => GetHelper(state, null, null, null);
        public static IConnect GetHelper(NetState state, HttpClient client) => GetHelper(state, null, null, client);
        public static IConnect GetHelper(NetState state, string username, string password) => GetHelper(state, username, password, null);
        public static IConnect GetHelper(NetState state, string username, string password, HttpClient client)
        {
            switch (state)
            {
                case NetState.Net:
                    return new NetHelper(username, password, client);
                case NetState.Auth4:
                    return new Auth4Helper(username, password, client);
                case NetState.Auth6:
                    return new Auth6Helper(username, password, client);
                default:
                    return null;
            }
        }
    }
}
