Imports System.Net.Http
Imports MvvmHelpers

Public Class NetCredential
    Inherits ObservableObject

    Private _State As NetState
    Public Property State As NetState
        Get
            Return _State
        End Get
        Set(value As NetState)
            SetProperty(_State, value)
        End Set
    End Property

    Private _Username As String
    Public Property Username As String
        Get
            Return _Username
        End Get
        Set(value As String)
            SetProperty(_Username, value)
        End Set
    End Property

    Private _Password As String
    Public Property Password As String
        Get
            Return _Password
        End Get
        Set(value As String)
            SetProperty(_Password, value)
        End Set
    End Property

    Private Shared ReadOnly Client As New HttpClient()

    Public Function GetHelper() As IConnect
        Return ConnectHelper.GetHelper(State, Username, Password, Client)
    End Function

    Public Function GetUseregHelper() As UseregHelper
        Return New UseregHelper(Username, Password, Client)
    End Function
End Class
