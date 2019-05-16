Imports TsinghuaNet.Helper
Imports TsinghuaNet.UWP.Helper
Imports Windows.ApplicationModel.Background

Public NotInheritable Class LiveTileTask
    Implements IBackgroundTask

    Public Async Sub Run(taskInstance As IBackgroundTaskInstance) Implements IBackgroundTask.Run
        Dim deferral = taskInstance.GetDeferral()
        Try
            Dim status As New InternetStatus
            Await status.RefreshAsync()
            Dim helper = ConnectHelper.GetHelper(Await status.SuggestAsync())
            If helper IsNot Nothing Then
                Using helper
                    Dim user As FluxUser = Await helper.GetFluxAsync()
                    NotificationHelper.UpdateTile(user)
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
