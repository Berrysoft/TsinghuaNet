Imports TsinghuaNet
Imports TsinghuaNet.Helper

Public Class MainViewModel
    Inherits NetViewModel

    Public Sub New()
        MyBase.New()

    End Sub

    Public Overrides Sub LoadSettings()

    End Sub

    Public Overrides Sub SaveSettings()

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
