using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TsinghuaNet.Models
{
    public abstract class MainViewModelBase : NetViewModelBase
    {
        public MainViewModelBase()
        {
            LoadSettings();
            RefreshStatusCommand = new Command(this, RefreshStatus);
            LoginCommand = new NetCommand(this, LoginAsync);
            LogoutCommand = new NetCommand(this, LogoutAsync);
            RefreshCommand = new NetCommand(this, RefreshAsync);
            Credential.PropertyChanged += OnCredentialStateChanged;
        }

        public abstract void LoadSettings();

        public abstract void SaveSettings();

        private void OnCredentialStateChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "State")
                Refresh();
        }

        public INetStatus Status { get; set; }
        private void OnStatusChanged() => RefreshStatus();

        public ICommand RefreshStatusCommand { get; }
        public async void RefreshStatus() => await RefreshStatusAsync();
        public async Task RefreshStatusAsync()
        {
            try
            {
                IsBusy = true;
                await Status.RefreshAsync();
                Credential.State = await Status.SuggestAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        public FluxUser OnlineUser { get; set; }

        public TimeSpan OnlineTime { get; set; }

        public string Response { get; set; }

        public event EventHandler<LogResponse> ReceivedResponse;

        protected virtual void OnReceivedResponse(LogResponse res) => ReceivedResponse?.Invoke(this, res);

        internal async Task NetCommandExecuteAsync(Func<IConnect, Task<LogResponse>> executor)
        {
            try
            {
                IsBusy = true;
                var helper = Credential.GetHelper();
                OnReceivedResponse(await executor(helper));
            }
            catch (Exception ex)
            {
                OnReceivedResponse(new LogResponse(false, ex.Message));
            }
            finally
            {
                IsBusy = false;
            }
        }

        public ICommand LoginCommand { get; }
        protected virtual async Task<LogResponse> LoginAsync(IConnect helper)
        {
            LogResponse res = new LogResponse(true, "登录成功");
            if (helper != null)
            {
                var r = await helper.LoginAsync();
                if (!r.Succeed)
                    res = r;
            }
            await RefreshAsync(helper);
            return res;
        }
        public Task LoginAsync() => NetCommandExecuteAsync(LoginAsync);
        public async void Login() => await LoginAsync();

        public ICommand LogoutCommand { get; }
        protected virtual async Task<LogResponse> LogoutAsync(IConnect helper)
        {
            LogResponse res = new LogResponse(true, "注销成功");
            if (helper != null)
            {
                var r = await helper.LogoutAsync();
                if (!r.Succeed)
                    res = r;
            }
            await RefreshAsync(helper);
            return res;
        }
        public Task LogoutAsync() => NetCommandExecuteAsync(LogoutAsync);
        public async void Logout() => await LogoutAsync();

        public ICommand RefreshCommand { get; }
        protected virtual async Task<LogResponse> RefreshAsync(IConnect helper)
        {
            FluxUser user = default;
            if (helper != null)
                user = await helper.GetFluxAsync();
            OnlineUser = user;
            return new LogResponse(true, string.Empty);
        }
        public Task RefreshAsync() => NetCommandExecuteAsync(RefreshAsync);
        public async void Refresh() => await RefreshAsync();
    }
}
