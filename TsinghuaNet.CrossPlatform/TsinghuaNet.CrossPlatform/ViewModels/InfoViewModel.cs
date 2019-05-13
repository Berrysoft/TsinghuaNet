using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TsinghuaNet.CrossPlatform.Services;
using TsinghuaNet.Helper;

namespace TsinghuaNet.CrossPlatform.ViewModels
{
    public class InfoViewModel : BaseViewModel
    {
        public InfoViewModel()
        {
            Title = "简要信息";

            LoginCommand = new NetCommand(Credential, LoginAsync);
            LogoutCommand = new NetCommand(Credential, LogoutAsync);
            RefreshCommand = new NetCommand(Credential, RefreshAsync);
        }

        private bool useAuth4;
        public bool UseAuth4
        {
            get => useAuth4;
            set => SetProperty(ref useAuth4, value, onChanged: OnUseAuth4Changed);
        }
        private void OnUseAuth4Changed()
        {
            if (useAuth4)
                Credential.State = NetState.Auth4;
            else
                Credential.State = NetState.Net;
            if (RefreshCommand.CanExecute(null))
                RefreshCommand.Execute(null);
        }

        private FluxUser onlineUser;
        public FluxUser OnlineUser
        {
            get => onlineUser;
            set => SetProperty(ref onlineUser, value, onChanged: OnOnlineUserChanged);
        }
        private void OnOnlineUserChanged() => OnlineTime = OnlineUser.OnlineTime;

        private TimeSpan onlineTime;
        public TimeSpan OnlineTime
        {
            get => onlineTime;
            set => SetProperty(ref onlineTime, value);
        }

        public ICommand LoginCommand { get; }
        private async Task<LogResponse> LoginAsync(IConnect helper)
        {
            if (helper != null)
            {
                var res = await helper.LoginAsync();
                if (!res.Succeed)
                    return res;
            }
            await RefreshAsync(helper);
            return new LogResponse(true, "登录成功");
        }

        public ICommand LogoutCommand { get; }
        private async Task<LogResponse> LogoutAsync(IConnect helper)
        {
            if (helper != null)
            {
                var res = await helper.LogoutAsync();
                if (!res.Succeed)
                    return res;
            }
            await RefreshAsync(helper);
            return new LogResponse(true, "注销成功");
        }

        public ICommand RefreshCommand { get; }
        private async Task<LogResponse> RefreshAsync(IConnect helper)
        {
            FluxUser user = default;
            if (helper != null)
                user = await helper.GetFluxAsync();
            OnlineUser = user;
            OnlineTime = user.OnlineTime;
            return new LogResponse(true, string.Empty);
        }
    }
}
