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
                NetCredential credential = new NetCredential();
                (credential.Username, credential.Password) = await CredentialStore.LoadCredentialAsync();
                InternetStatus status = new InternetStatus();
                credential.State = await status.SuggestAsync();
                NetXFSettings settings = new NetXFSettings();
                settings.LoadSettings();
                var helper = credential.GetHelper(settings);
                if (helper != null)
                {
                    if (settings.EnableRelogin) await helper.LogoutAsync();
                    await helper.LoginAsync();
                    FluxUser user = await helper.GetFluxAsync();
                    NotificationHelper.UpdateTile(user);
                    NotificationHelper.SendToast(user);
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
