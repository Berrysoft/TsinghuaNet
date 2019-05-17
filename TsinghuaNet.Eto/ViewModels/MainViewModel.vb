Imports Eto.Forms
Imports TsinghuaNet.Helper

Public Class MainViewModel
    Inherits NetViewModel

    Public Sub New()
        MyBase.New()
        Status = New NetPingStatus()
        timer = New UITimer(AddressOf OnlineTimerTick)
        timer.Interval = 1
    End Sub

    Public Overrides Sub LoadSettings()

    End Sub

    Public Overrides Sub SaveSettings()

    End Sub

    Protected Overrides Sub OnSuggestStateChanged()
        MyBase.OnSuggestStateChanged()
        Credential.State = SuggestState
    End Sub

    Private _OnlineTime As TimeSpan
    Public Property OnlineTime As TimeSpan
        Get
            Return _OnlineTime
        End Get
        Set(value As TimeSpan)
            SetProperty(_OnlineTime, value)
        End Set
    End Property

    Private timer As UITimer
    Private Sub OnlineTimerTick()
        OnlineTime += TimeSpan.FromSeconds(1)
    End Sub

    Protected Overrides Async Function RefreshAsync(helper As IConnect) As Task(Of LogResponse)
        Dim res = Await MyBase.RefreshAsync(helper)
        timer.Stop()
        If Not String.IsNullOrEmpty(OnlineUser.Username) Then
            timer.Start()
        End If
        Return res
    End Function

    Private _Response As String
    Public Property Response As String
        Get
            Return _Response
        End Get
        Set(value As String)
            SetProperty(_Response, value)
        End Set
    End Property

    Private Sub Model_ReceivedResponse(sender As Object, res As LogResponse) Handles Me.ReceivedResponse
        Response = res.Message
    End Sub
End Class
