using System.ComponentModel;
using System.Net.Http;

namespace TsinghuaNet.Models
{
    public class NetCredential : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public NetState State { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        private static readonly HttpClient Client = new HttpClient();

        public IConnect GetHelper() => ConnectHelper.GetHelper(State, Username, Password, Client);

        public IUsereg GetUseregHelper() => new UseregHelper(Username, Password, Client);
    }
}
