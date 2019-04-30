using System;
using System.Collections.Generic;
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
        private static readonly Dictionary<NetState, Type> StateHelperMap = new Dictionary<NetState, Type>
        {
            [NetState.Net] = typeof(NetHelper),
            [NetState.Auth4] = typeof(Auth4Helper),
            [NetState.Auth6] = typeof(Auth6Helper)
        };

        private static IConnect GetHelperImpl(NetState state, params object[] args)
        {
            if (StateHelperMap.ContainsKey(state))
            {
                return (IConnect)Activator.CreateInstance(StateHelperMap[state], args);
            }
            else
            {
                return null;
            }
        }

        public static IConnect GetHelper(NetState state) => GetHelperImpl(state);

        public static IConnect GetHelper(NetState state, HttpClient client) => GetHelperImpl(state, client);

        public static IConnect GetHelper(NetState state, string username, string password) => GetHelperImpl(state, username, password);

        public static IConnect GetHelper(NetState state, string username, string password, HttpClient client) => GetHelperImpl(state, username, password, client);
    }
}
