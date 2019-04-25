Imports Berrysoft.Tsinghua.Net
Imports TsinghuaNetUWP.Helper
Imports Windows.ApplicationModel.Background

Public NotInheritable Class LiveTileTask
    Implements IBackgroundTask

    Public Async Sub Run(taskInstance As IBackgroundTaskInstance) Implements IBackgroundTask.Run
        Dim deferral = taskInstance.GetDeferral()
        Try
            Dim tuple = SettingsHelper.GetInternetStatus()
            Dim state = SettingsHelper.SuggestNetState(tuple.Status, tuple.Ssid)
            Dim helper = ConnectHelper.GetHelper(state)
            If helper IsNot Nothing Then
                Using helper
                    Dim user As FluxUser = Await helper.GetFluxAsync()
                    NotificationHelper.UpdateTile(user)
                End Using
            End If
        Finally
            deferral.Complete()
        End Try
    End Sub
End Class
