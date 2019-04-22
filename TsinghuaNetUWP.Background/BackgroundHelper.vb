Imports Windows.ApplicationModel.Background

Public Module BackgroundHelper
    Private Async Function RequestAccessImplAsync() As Task(Of Boolean)
        Dim status = Await BackgroundExecutionManager.RequestAccessAsync()
        Return Not (
            status = BackgroundAccessStatus.Unspecified OrElse
            status = BackgroundAccessStatus.DeniedByUser OrElse
            status = BackgroundAccessStatus.DeniedBySystemPolicy)
    End Function

    Public Function RequestAccessAsync() As IAsyncOperation(Of Boolean)
        Return RequestAccessImplAsync().AsAsyncOperation()
    End Function

    Private Sub UnregisterTask(name As String)
        For Each v In From t In BackgroundTaskRegistration.AllTasks
                      Let r = t.Value Where r.Name = name Select r
            v.Unregister(True)
        Next
    End Sub

    Private Sub RegisterTask(name As String, type As Type, Optional trigger As IBackgroundTrigger = Nothing, Optional condition As IBackgroundCondition = Nothing)
        Dim task As New BackgroundTaskBuilder
        task.Name = name
        task.TaskEntryPoint = type.FullName
        If trigger IsNot Nothing Then task.SetTrigger(trigger)
        If condition IsNot Nothing Then task.AddCondition(condition)
        task.Register()
    End Sub

    Private Const LIVETILETASK As String = "LIVETILETASK"

    Public Sub RegisterLiveTile(reg As Boolean)
        UnregisterTask(LIVETILETASK)
        If reg Then
            RegisterTask(LIVETILETASK, GetType(LiveTileTask), New TimeTrigger(15, False), New SystemCondition(SystemConditionType.InternetAvailable))
        End If
    End Sub

    Private Const LOGINTASK As String = "LOGINTASK"

    Public Sub RegisterLogin(reg As Boolean)
        UnregisterTask(LOGINTASK)
        If reg Then
            RegisterTask(LOGINTASK, GetType(LoginTask), New SystemTrigger(SystemTriggerType.NetworkStateChange, False))
        End If
    End Sub
End Module
