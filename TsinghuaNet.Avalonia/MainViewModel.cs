using System;
using System.Threading.Tasks;
using MvvmHelpers;

namespace TsinghuaNet.Avalonia
{
    class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            StateChangeCommand = new NetStateChangeCommand(this);
            LoginCommand = new LoginCommand(this);
            LogoutCommand = new LogoutCommand(this);
            RefreshCommand = new RefreshCommand(this);
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

        private NetState state;
        public NetState State
        {
            get => state;
            set => SetProperty(ref state, value, onChanged: OnStateChanged);
        }
        private void OnStateChanged()
        {
            if (RefreshCommand.CanExecute(null))
            {
                RefreshCommand.Execute(null);
            }
        }

        public NetStateChangeCommand StateChangeCommand { get; }

        private string onlineUsername;
        public string OnlineUsername
        {
            get => onlineUsername;
            set => SetProperty(ref onlineUsername, value);
        }

        private long onlineFlux;
        public long OnlineFlux
        {
            get => onlineFlux;
            set => SetProperty(ref onlineFlux, value);
        }

        private TimeSpan onlineTime;
        public TimeSpan OnlineTime
        {
            get => onlineTime;
            set => SetProperty(ref onlineTime, value);
        }

        private decimal onlineBalance;
        public decimal OnlineBalance
        {
            get => onlineBalance;
            set => SetProperty(ref onlineBalance, value);
        }

        public NetCommand LoginCommand { get; }
        public NetCommand LogoutCommand { get; }
        public NetCommand RefreshCommand { get; }

        internal IConnect GetHelper() => ConnectHelper.GetHelper(State, Username, Password);

        internal async Task RefreshAsync(IConnect helper)
        {
            FluxUser flux = default;
            if (helper != null)
                flux = await helper.GetFluxAsync();
            OnlineUsername = flux.Username;
            OnlineFlux = flux.Flux;
            OnlineTime = flux.OnlineTime;
            OnlineBalance = flux.Balance;
        }
    }
}
