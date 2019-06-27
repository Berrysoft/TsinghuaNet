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

        private const string LIVETILETASK = "LIVETILETASK";

        public static void RegisterLiveTile(bool reg)
        {
            if (reg)
                BackgroundTaskHelper.Register(LIVETILETASK, typeof(LiveTileTask).FullName, new TimeTrigger(15, false), true, true, new SystemCondition(SystemConditionType.InternetAvailable));
            else
                BackgroundTaskHelper.Unregister(LIVETILETASK);
        }

        private const string LOGINTASK = "LOGINTASK";

        public static void RegisterLogin(bool reg)
        {
            if (reg)
                BackgroundTaskHelper.Register(LOGINTASK, typeof(LoginTask).FullName, new SystemTrigger(SystemTriggerType.NetworkStateChange, false), true, true);
            else
                BackgroundTaskHelper.Unregister(LOGINTASK);
        }
    }
}
