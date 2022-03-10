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

        private static readonly HttpClient Client = new HttpClient();

        public IConnect GetHelper(NetSettingsBase settings)
        {
            return State switch
            {
                NetState.Net => new NetHelper(Username, Password, Client),
                NetState.Auth4 => new Auth4Helper(Username, Password, Client, settings),
                NetState.Auth6 => new Auth6Helper(Username, Password, Client, settings),
                _ => null,
            };
        }

        public IUsereg GetUseregHelper() => new UseregHelper(Username, Password, Client);
    }
}
