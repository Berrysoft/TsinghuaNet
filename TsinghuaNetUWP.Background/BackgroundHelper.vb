Imports Windows.ApplicationModel.Background
Imports Microsoft.Toolkit.Uwp.Helpers

Public Module BackgroundHelper
    Private Async Function RequestAccessImplAsync() As Task(Of Boolean)
        Dim status = Await BackgroundExecutionManager.RequestAccessAsync()
        Return Not (status = BackgroundAccessStatus.Unspecified OrElse
                    status = BackgroundAccessStatus.DeniedByUser OrElse
                    status = BackgroundAccessStatus.DeniedBySystemPolicy)
    End Function

    Public Function RequestAccessAsync() As IAsyncOperation(Of Boolean)
        Return RequestAccessImplAsync().AsAsyncOperation()
    End Function

    Private Const LIVETILETASK As String = "LIVETILETASK"

    Public Sub RegisterLiveTile(reg As Boolean)
        If reg Then
            BackgroundTaskHelper.Register(LIVETILETASK, GetType(LiveTileTask).FullName, New TimeTrigger(15, False), True, True, New SystemCondition(SystemConditionType.InternetAvailable))
        Else
            BackgroundTaskHelper.Unregister(LIVETILETASK)
        End If
    End Sub

    Private Const LOGINTASK As String = "LOGINTASK"

    Public Sub RegisterLogin(reg As Boolean)
        If reg Then
            BackgroundTaskHelper.Register(LOGINTASK, GetType(LoginTask).FullName, New SystemTrigger(SystemTriggerType.NetworkStateChange, False), True, True)
        Else
            BackgroundTaskHelper.Unregister(LOGINTASK)
        End If
    End Sub
End Module
