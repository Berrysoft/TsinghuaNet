using TsinghuaNet.Models;
using TsinghuaNet.XF.Models;
using TsinghuaNet.XF.UWP.Helpers;
using Windows.ApplicationModel.Background;

namespace TsinghuaNet.XF.UWP.Background
{
    public sealed class LoginTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            try
            {
                InternetStatus status = new InternetStatus();
                status.Refresh();
                CredentialStore store = new CredentialStore();
                NetCredential credential = new NetCredential();
                await store.LoadCredentialAsync(credential);
                credential.State = await status.SuggestAsync();
                var helper = credential.GetHelper();
                if (helper != null)
                {
                    await helper.LoginAsync();
                    FluxUser user = await helper.GetFluxAsync();
                    NotificationHelper.UpdateTile(user);
                    NotificationHelper.SendToast(user);
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
