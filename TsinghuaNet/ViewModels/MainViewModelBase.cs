using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using PropertyChanged;
using TsinghuaNet.Models;

namespace TsinghuaNet.ViewModels
{
    public abstract class MainViewModelBase : NetViewModelBase
    {
        public MainViewModelBase()
        {
            RefreshStatusCommand = new Command(this, RefreshStatus);
            LoginCommand = new NetCommand(this, LoginAsync);
            LogoutCommand = new NetCommand(this, LogoutAsync);
            RefreshCommand = new NetCommand(this, RefreshAsync);
            Credential.PropertyChanged += OnCredentialStateChanged;
        }

        public abstract void LoadSettings();
        public abstract void SaveSettings();

        [SuppressPropertyChangedWarnings]
        private void OnCredentialStateChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "State")
                Refresh();
        }

        protected override void OnStatusChanged() => RefreshStatus();

        public ICommand RefreshStatusCommand { get; }
        public async void RefreshStatus() => await RefreshStatusAsync();
        public async Task RefreshStatusAsync()
        {
            try
            {
                IsBusy = true;
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

        internal async Task NetCommandExecuteAsync(Func<IConnect, Task<LogResponse>> executor)
        {
            try
            {
                IsBusy = true;
                var helper = Credential.GetHelper(Settings);
                Response = (await executor(helper)).Message;
            }
            catch (Exception ex)
            {
                Response = ex.Message;
                Console.Error.WriteLine(ex);
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
                if (Settings.EnableRelogin) await helper.LogoutAsync();
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
            var remainFlux = FluxHelper.GetMaxFlux(OnlineUser.Flux, OnlineUser.Balance) - OnlineUser.Flux;
            if (Settings.EnableFluxLimit && remainFlux < Settings.FluxLimit)
                return new LogResponse(false, $"流量仅剩余{remainFlux}");
            else
                return new LogResponse(true, string.Empty);
        }
        public Task RefreshAsync() => NetCommandExecuteAsync(RefreshAsync);
        public async void Refresh() => await RefreshAsync();
    }
}
