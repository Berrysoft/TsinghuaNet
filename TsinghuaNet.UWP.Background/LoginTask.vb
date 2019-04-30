Imports Berrysoft.Tsinghua.Net
Imports TsinghuaNet.UWP.Helper
Imports Windows.ApplicationModel.Background

Public NotInheritable Class LoginTask
    Implements IBackgroundTask

    Public Async Sub Run(taskInstance As IBackgroundTaskInstance) Implements IBackgroundTask.Run
        Dim deferral = taskInstance.GetDeferral()
        Try
            Dim tuple = SettingsHelper.GetInternetStatus()
            Dim state = SettingsHelper.SuggestNetState(tuple.Status, tuple.Ssid)
            Dim un = SettingsHelper.StoredUsername
            Dim pw = CredentialHelper.GetCredential(un)
            Dim helper = ConnectHelper.GetHelper(state, un, pw)
            If helper IsNot Nothing Then
                Using helper
                    Await helper.LoginAsync()
                    Dim user As FluxUser = Await helper.GetFluxAsync()
                    NotificationHelper.UpdateTile(user)
                    NotificationHelper.SendToast(user)
                End Using
            End If
        Finally
            deferral.Complete()
        End Try
    End Sub
End Class
