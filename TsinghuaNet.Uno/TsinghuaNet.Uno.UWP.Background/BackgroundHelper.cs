using System;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.ApplicationModel.Background;
using Windows.Foundation;

namespace TsinghuaNet.Uno.UWP.Background
{
    public static class BackgroundHelper
    {
        private async static Task<bool> RequestAccessImplAsync()
        {
            var status = await BackgroundExecutionManager.RequestAccessAsync();
            return !(status == BackgroundAccessStatus.Unspecified || status == BackgroundAccessStatus.DeniedByUser || status == BackgroundAccessStatus.DeniedBySystemPolicy);
        }

        public static IAsyncOperation<bool> RequestAccessAsync()
        {
            return RequestAccessImplAsync().AsAsyncOperation();
        }

        public static void RegisterLiveTile(bool reg)
        {
            if (reg)
                BackgroundTaskHelper.Register(typeof(LiveTileTask), new TimeTrigger(15, false), true, true, new SystemCondition(SystemConditionType.InternetAvailable));
            else
                BackgroundTaskHelper.Unregister(typeof(LiveTileTask));
        }

        public static void RegisterLogin(bool reg)
        {
            if (reg)
                BackgroundTaskHelper.Register(typeof(LoginTask), new SystemTrigger(SystemTriggerType.NetworkStateChange, false), true, true);
            else
                BackgroundTaskHelper.Unregister(typeof(LoginTask));
        }
    }
}
