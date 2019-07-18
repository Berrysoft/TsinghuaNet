using TsinghuaNet.Models;
using TsinghuaNet.XF.Models;
using TsinghuaNet.XF.UWP.Helpers;
using Windows.ApplicationModel.Background;

namespace TsinghuaNet.XF.UWP.Background
{
    public sealed class LiveTileTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            try
            {
                InternetStatus status = new InternetStatus();
                status.Refresh();
                NetCredential credential = new NetCredential();
                credential.State = await status.SuggestAsync();
                var helper = credential.GetHelper();
                if (helper != null)
                {
                    FluxUser user = await helper.GetFluxAsync();
                    NotificationHelper.UpdateTile(user);
                    NetXFSettings settings = new NetXFSettings();
                    settings.LoadSettings();
                    if (settings.EnableFluxLimit)
                        NotificationHelper.SendWarningToast(user, settings.FluxLimit);
                }
            }
            finally
            {
                deferral.Complete();
            }
        }
    }
}
