using System;
using System.Collections.Generic;
using System.Text;

namespace TsinghuaNet.CrossPlatform.ViewModels
{
    public class InfoViewModel : BaseViewModel
    {
        public InfoViewModel()
        {
            Title = "简要信息";
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
    }
}
