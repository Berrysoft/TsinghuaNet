using TsinghuaNet.Helpers;
using TsinghuaNet.Uno.Helpers;
using Windows.ApplicationModel.Background;

namespace TsinghuaNet.Uno.UWP.Background
{
    public sealed class LoginTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            try
            {
                InternetStatus status = new InternetStatus();
                await status.RefreshAsync();
                var un = SettingsHelper.StoredUsername;
                var pw = CredentialHelper.GetCredential(un);
                var helper = ConnectHelper.GetHelper(await status.SuggestAsync(), un, pw);
                if (helper != null)
                {
                    using (helper)
                    {
                        await helper.LoginAsync();
                        FluxUser user = await helper.GetFluxAsync();
                        NotificationHelper.UpdateTile(user);
                        NotificationHelper.SendToast(user);
                        if (SettingsHelper.FluxLimit != null)
                            NotificationHelper.SendWarningToast(user, SettingsHelper.FluxLimit.Value);
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
