Imports System.IO
Imports System.Net.Http
Imports System.Text
Imports Avalonia.Threading
Imports MvvmHelpers
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports TsinghuaNet.Helper

Public Class MainViewModel
    Inherits ObservableObject

    Private Const settingsFilename As String = "settings.json"

    Private Shared Function GetSettings(json As JObject, key As String, def As JToken) As JToken
        If json.ContainsKey(key) Then
            Return json(key)
        Else
            Return def
        End If
    End Function

    Public Async Function LoadSettingsAsync() As Task
        If File.Exists(settingsFilename) Then
            Using stream As New StreamReader(settingsFilename)
                Using reader As New JsonTextReader(stream)
                    Dim json = Await JObject.LoadAsync(reader)
                    Username = GetSettings(json, "username", String.Empty)
                    Password = Encoding.UTF8.GetString(Convert.FromBase64String(GetSettings(json, "password", String.Empty)))
                    State = CInt(GetSettings(json, "state", NetState.Unknown))
                    AutoLogin = CBool(GetSettings(json, "autologin", True))
                    AutoSuggest = CBool(GetSettings(json, "autosuggest", True))
                    UseTimer = CBool(GetSettings(json, "usetimer", True))
                End Using
            End Using
        End If
        If AutoSuggest Then
            State = Await SuggestionHelper.GetSuggestion()
        End If
        If AutoLogin AndAlso LoginCommand.CanExecute(Nothing) Then
            LoginCommand.Execute(Nothing)
        ElseIf RefreshCommand.CanExecute(Nothing) Then
            RefreshCommand.Execute(Nothing)
        End If
    End Function

    Public Sub SaveSettings()
        Dim json As New JObject
        json("username") = If(Username, String.Empty)
        json("password") = Convert.ToBase64String(Encoding.UTF8.GetBytes(If(Password, String.Empty)))
        json("state") = State
        json("autologin") = AutoLogin
        json("autosuggest") = AutoSuggest
        json("usetimer") = UseTimer
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

    Private _ConnectionSuccess As Boolean
    Public Property ConnectionSuccess As Boolean
        Get
            Return _ConnectionSuccess
        End Get
        Set(value As Boolean)
            SetProperty(_ConnectionSuccess, value)
        End Set
    End Property

    Private _FailMessage As String
    Public Property FailMessage As String
        Get
            Return _FailMessage
        End Get
        Set(value As String)
            SetProperty(_FailMessage, value)
        End Set
    End Property

    Private _AutoLogin As Boolean
    Public Property AutoLogin As Boolean
        Get
            Return _AutoLogin
        End Get
        Set(value As Boolean)
            SetProperty(_AutoLogin, value)
        End Set
    End Property

    Private _AutoSuggest As Boolean
    Public Property AutoSuggest As Boolean
        Get
            Return _AutoSuggest
        End Get
        Set(value As Boolean)
            SetProperty(_AutoSuggest, value)
        End Set
    End Property

    Private _UseTimer As Boolean
    Public Property UseTimer As Boolean
        Get
            Return _UseTimer
        End Get
        Set(value As Boolean)
            SetProperty(_UseTimer, value)
        End Set
    End Property

    Private _OnlineUser As FluxUser
    Public Property OnlineUser As FluxUser
        Get
            Return _OnlineUser
        End Get
        Set(value As FluxUser)
            SetProperty(_OnlineUser, value, onChanged:=AddressOf OnOnlineUserChanged)
        End Set
    End Property
    Private Sub OnOnlineUserChanged()
        OnlineTimeTimer.Stop()
        If _UseTimer AndAlso Not String.IsNullOrEmpty(OnlineUser.Username) Then
            OnlineTimeTimer.Start()
        End If
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

    Friend OnlineTimeTimer As New DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.DataBind, AddressOf OnlineTimeTimer_Tick)

    Private Sub OnlineTimeTimer_Tick(sender As Object, e As EventArgs)
        OnlineTime += TimeSpan.FromSeconds(1)
    End Sub

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
        FluxOffset = flux.Flux / FluxHelper.GetMaxFlux(flux.Flux, flux.Balance)
    End Function

    Private _FluxOffset As Double
    Public Property FluxOffset As Double
        Get
            Return _FluxOffset
        End Get
        Set(value As Double)
            SetProperty(_FluxOffset, value)
        End Set
    End Property

    Public ReadOnly Property ShowConnectionCommand As New ShowDialogCommand(Of ConnectionWindow)(Me)
    Public ReadOnly Property ShowDetailCommand As New ShowDialogCommand(Of DetailWindow)(Me)
    Public ReadOnly Property ShowSettingsCommand As New ShowDialogCommand(Of SettingsWindow)(Me)
End Class
