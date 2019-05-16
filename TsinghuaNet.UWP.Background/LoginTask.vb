Imports TsinghuaNet.Helper
Imports TsinghuaNet.UWP.Helper
Imports Windows.ApplicationModel.Background

Public NotInheritable Class LoginTask
    Implements IBackgroundTask

    Public Async Sub Run(taskInstance As IBackgroundTaskInstance) Implements IBackgroundTask.Run
        Dim deferral = taskInstance.GetDeferral()
        Try
            Dim status As New InternetStatus
            Await status.RefreshAsync()
            Dim un = SettingsHelper.StoredUsername
            Dim pw = CredentialHelper.GetCredential(un)
            Dim helper = ConnectHelper.GetHelper(Await status.SuggestAsync(), un, pw)
            If helper IsNot Nothing Then
                Using helper
                    Await helper.LoginAsync()
                    Dim user As FluxUser = Await helper.GetFluxAsync()
                    NotificationHelper.UpdateTile(user)
                    NotificationHelper.SendToast(user)
                    If SettingsHelper.FluxLimit IsNot Nothing Then
                        NotificationHelper.SendWarningToast(user, SettingsHelper.FluxLimit)
                    End If
                End Using
            End If
        Finally
            deferral.Complete()
        End Try
    End Sub
End Class
