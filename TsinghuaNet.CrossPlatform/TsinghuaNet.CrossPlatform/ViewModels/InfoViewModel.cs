using System;
using System.Threading.Tasks;
using TsinghuaNet.CrossPlatform.Models;
using TsinghuaNet.Helper;

namespace TsinghuaNet.CrossPlatform.ViewModels
{
    public class InfoViewModel : NetViewModel
    {
        public InfoViewModel() : base()
        {
            timer = new DeviceTimer(TimeSpan.FromSeconds(1), OnlineTimerTick);
        }

        public override void LoadSettings() { }

        public override void SaveSettings() { }

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
            Refresh();
        }

        private TimeSpan onlineTime;
        public TimeSpan OnlineTime
        {
            get => onlineTime;
            set => SetProperty(ref onlineTime, value);
        }

        private DeviceTimer timer;
        private void OnlineTimerTick(object sender, EventArgs e)
        {
            OnlineTime += TimeSpan.FromSeconds(1);
            if (string.IsNullOrEmpty(OnlineUser.Username))
                timer.Stop();
        }

        protected override async Task<LogResponse> RefreshAsync(IConnect helper)
        {
            var res = await base.RefreshAsync(helper);
            onlineTime = OnlineUser.OnlineTime;
            timer.Start();
            return res;
        }
    }
}
