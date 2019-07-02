using TsinghuaNet.Helpers;
using TsinghuaNet.Uno.Helpers;
using Windows.ApplicationModel.Background;

namespace TsinghuaNet.Uno.UWP.Background
{
    public sealed class LiveTileTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            try
            {
                InternetStatus status = new InternetStatus();
                NetSettings settings = new NetSettings();
                await status.RefreshAsync();
                var helper = ConnectHelper.GetHelper(await status.SuggestAsync());
                if (helper != null)
                {
                    using (helper)
                    {
                        FluxUser user = await helper.GetFluxAsync();
                        NotificationHelper.UpdateTile(user);
                        if (settings.EnableFluxLimit)
                            NotificationHelper.SendWarningToast(user, settings.FluxLimit);
                    }
                }
            }
            finally
            {
                deferral.Complete();
            }
        }
    }
}
