using System;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;
using TsinghuaNet.XF.Services;
using TsinghuaNet.XF.UWP.Background;
using TsinghuaNet.XF.UWP.Services;
using Windows.ApplicationModel.Background;
using Xamarin.Forms;

[assembly: Dependency(typeof(BackgroundManager))]

namespace TsinghuaNet.XF.UWP.Services
{
    public class BackgroundManager : IBackgroundManager
    {
        public async Task<bool> RequestAccessAsync()
        {
            var status = await BackgroundExecutionManager.RequestAccessAsync();
            return !(status == BackgroundAccessStatus.Unspecified || status == BackgroundAccessStatus.DeniedByUser || status == BackgroundAccessStatus.DeniedBySystemPolicy);
        }

        public void RegisterLiveTile(bool reg)
        {
            if (reg)
                BackgroundTaskHelper.Register(typeof(LiveTileTask), new TimeTrigger(15, false), true, true, new SystemCondition(SystemConditionType.InternetAvailable));
            else
                BackgroundTaskHelper.Unregister(typeof(LiveTileTask));
        }

        public void RegisterLogin(bool reg)
        {
            if (reg)
                BackgroundTaskHelper.Register(typeof(LoginTask), new SystemTrigger(SystemTriggerType.NetworkStateChange, false), true, true);
            else
                BackgroundTaskHelper.Unregister(typeof(LoginTask));
        }
    }
}
