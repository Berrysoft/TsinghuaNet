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
                NetCredential credential = new NetCredential();
                InternetStatus status = new InternetStatus();
                credential.State = await status.SuggestAsync();
                NetXFSettings settings = new NetXFSettings();
                settings.LoadSettings();
                credential.UseProxy = settings.UseProxy;
                var helper = credential.GetHelper();
                if (helper != null)
                {
                    FluxUser user = await helper.GetFluxAsync();
                    NotificationHelper.UpdateTile(user);
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
