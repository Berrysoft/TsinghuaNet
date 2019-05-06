Imports System.IO
Imports System.Net.Http
Imports System.Text
Imports MvvmHelpers
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports TsinghuaNet.Helper

Public Class MainViewModel
    Inherits ObservableObject

    Private Const settingsFilename As String = "settings.json"

    Public Sub New()
        If File.Exists(settingsFilename) Then
            Try
                Using stream As New StreamReader(settingsFilename)
                    Using reader As New JsonTextReader(stream)
                        Dim json = JObject.Load(reader)
                        _Username = json("username")
                        _Password = Encoding.UTF8.GetString(Convert.FromBase64String(json("password")))
                        _State = CInt(json("state"))
                    End Using
                End Using
            Catch ex As Exception

            End Try
        End If
    End Sub

    Public Sub SaveSettings()
        Dim json As New JObject
        json("username") = If(_Username, String.Empty)
        json("password") = Convert.ToBase64String(Encoding.UTF8.GetBytes(If(_Password, String.Empty)))
        json("state") = _State
        Using stream As New StreamWriter(settingsFilename)
            Using writer As New JsonTextWriter(stream)
                json.WriteTo(writer)
            End Using
        End Using
    End Sub

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

    Private _State As NetState
    Public Property State As NetState
        Get
            Return _State
        End Get
        Set(value As NetState)
            SetProperty(_State, value, onChanged:=AddressOf OnStateChanged)
        End Set
    End Property
    Private Sub OnStateChanged()
        If RefreshCommand.CanExecute(Nothing) Then
            RefreshCommand.Execute(Nothing)
        End If
    End Sub

    Public ReadOnly Property StateChangeCommand As New NetStateChangeCommand(Me)

    Private _OnlineUser As FluxUser
    Public Property OnlineUser As FluxUser
        Get
            Return _OnlineUser
        End Get
        Set(value As FluxUser)
            SetProperty(_OnlineUser, value)
        End Set
    End Property

    Private _OnlineTime As TimeSpan
    Public Property OnlineTime As TimeSpan
        Get
            Return _OnlineTime
        End Get
        Set(value As TimeSpan)
            SetProperty(_OnlineTime, value)
        End Set
    End Property

    Public ReadOnly Property LoginCommand As NetCommand = New LoginCommand(Me)
    Public ReadOnly Property LogoutCommand As NetCommand = New LogoutCommand(Me)
    Public ReadOnly Property RefreshCommand As NetCommand = New RefreshCommand(Me)

    Private Shared Client As New HttpClient

    Friend Function GetHelper() As IConnect
        Return ConnectHelper.GetHelper(State, Username, Password, Client)
    End Function

    Friend Function GetUseregHelper() As UseregHelper
        Return New UseregHelper(Username, Password, Client)
    End Function

    Friend Async Function RefreshAsync(helper As IConnect) As Task
        Dim flux As FluxUser = Nothing
        If helper IsNot Nothing Then
            flux = Await helper.GetFluxAsync()
        End If
        OnlineUser = flux
        OnlineTime = flux.OnlineTime
    End Function

    Public ReadOnly Property ShowConnectionCommand As New ShowDialogCommand(Of ConnectionWindow)(Me)
    Public ReadOnly Property ShowDetailCommand As New ShowDialogCommand(Of DetailWindow)(Me)
End Class
