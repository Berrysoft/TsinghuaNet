using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;

namespace TsinghuaNet.Models
{
    public class NetCredential : INotifyPropertyChanged
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        public NetState State { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool UseProxy { get; set; }

        private static readonly HttpClient Client = new HttpClient();
        private static readonly HttpClient NoProxyClient = new HttpClient(
#if NETCOREAPP
            new SocketsHttpHandler()
#else
            new HttpClientHandler()
#endif
            { UseProxy = false }
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private HttpClient GetClient() => UseProxy ? Client : NoProxyClient;

        public IConnect GetHelper()
        {
            return State switch
            {
                NetState.Net => new NetHelper(Username, Password, GetClient()),
                NetState.Auth4 => new Auth4Helper(Username, Password, GetClient()),
                NetState.Auth6 => new Auth6Helper(Username, Password, GetClient()),
                _ => null,
            };
        }

        public IUsereg GetUseregHelper() => new UseregHelper(Username, Password, GetClient());
    }
}
