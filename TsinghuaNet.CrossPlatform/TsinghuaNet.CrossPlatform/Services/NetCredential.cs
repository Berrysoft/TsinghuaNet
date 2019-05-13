using TsinghuaNet.Helper;
using MvvmHelpers;
using System.Net.Http;

namespace TsinghuaNet.CrossPlatform.Services
{
    public class NetCredential : ObservableObject
    {
        private NetState state;
        public NetState State
        {
            get => state;
            set => SetProperty(ref state, value);
        }

        private string username;
        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }

        private string password;
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        private static readonly HttpClient Client = new HttpClient();

        public IConnect GetHelper() => ConnectHelper.GetHelper(State, Username, Password, Client);
        public UseregHelper GetUseregHelper() => new UseregHelper(Username, Password, Client);
    }
}
