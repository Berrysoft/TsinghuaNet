using System.ComponentModel;
using System.Net.Http;

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

        public IConnect GetHelper()
        {
            return State switch
            {
                NetState.Net => new NetHelper(Username, Password, Client),
                NetState.Auth4 => new Auth4Helper(Username, Password, Client),
                NetState.Auth6 => new Auth6Helper(Username, Password, Client),
                _ => null,
            };
        }

        public IUsereg GetUseregHelper() => new UseregHelper(Username, Password, Client);
    }
}
