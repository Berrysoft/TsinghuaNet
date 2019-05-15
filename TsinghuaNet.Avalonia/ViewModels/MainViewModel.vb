Imports System.IO
Imports System.Text
Imports Avalonia.Media
Imports Avalonia.Threading
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports TsinghuaNet.Helper

Public Class MainViewModel
    Inherits NetViewModel

    Public Sub New()
        MyBase.New()
        Status = New NetPingStatus()
        If AutoLogin AndAlso LoginCommand.CanExecute(Nothing) Then
            LoginCommand.Execute(Nothing)
        ElseIf RefreshCommand.CanExecute(Nothing) Then
            RefreshCommand.Execute(Nothing)
        End If
    End Sub

    Protected Overrides Sub OnSuggestStateChanged()
        MyBase.OnSuggestStateChanged()
        Credential.State = SuggestState
    End Sub

    Private Const settingsFilename As String = "settings.json"

    Private Shared Function GetSettings(json As JObject, key As String, def As JToken) As JToken
        If json.ContainsKey(key) Then
            Return json(key)
        Else
            Return def
        End If
    End Function

    Public Overrides Sub LoadSettings()
        If File.Exists(settingsFilename) Then
            Using stream As New StreamReader(settingsFilename)
                Using reader As New JsonTextReader(stream)
                    Dim json = JObject.Load(reader)
                    Credential.Username = GetSettings(json, "username", String.Empty)
                    Credential.Password = Encoding.UTF8.GetString(Convert.FromBase64String(GetSettings(json, "password", String.Empty)))
                    Credential.State = CInt(GetSettings(json, "state", NetState.Unknown))
                    AutoLogin = CBool(GetSettings(json, "autoLogin", True))
                    AutoSuggest = CBool(GetSettings(json, "autoSuggest", True))
                    UseTimer = CBool(GetSettings(json, "useTimer", True))
                    EnableFluxLimit = CBool(GetSettings(json, "enableFluxLimit", True))
                    FluxLimit = CDbl(GetSettings(json, "fluxLimit", 20.0))
                End Using
            End Using
        End If
    End Sub

    Public Overrides Sub SaveSettings()
        Dim json As New JObject
        json("username") = If(Credential.Username, String.Empty)
        json("password") = Convert.ToBase64String(Encoding.UTF8.GetBytes(If(Credential.Password, String.Empty)))
        json("state") = Credential.State
        json("autoLogin") = AutoLogin
        json("autoSuggest") = AutoSuggest
        json("useTimer") = UseTimer
        json("enableFluxLimit") = EnableFluxLimit
        json("fluxLimit") = FluxLimit
        Using stream As New StreamWriter(settingsFilename)
            Using writer As New JsonTextWriter(stream)
                json.WriteTo(writer)
            End Using
        End Using
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

    Protected Overrides Async Function RefreshAsync(helper As IConnect) As Task(Of LogResponse)
        Dim res = Await MyBase.RefreshAsync(helper)
        OnlineTime = OnlineUser.OnlineTime
        FluxOffset = OnlineUser.Flux / FluxHelper.GetMaxFlux(OnlineUser.Flux, OnlineUser.Balance)
        OnlineTimeTimer.Stop()
        If _UseTimer AndAlso Not String.IsNullOrEmpty(OnlineUser.Username) Then
            OnlineTimeTimer.Start()
        End If
        Return res
    End Function

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

    Private _FluxOffset As Double
    Public Property FluxOffset As Double
        Get
            Return _FluxOffset
        End Get
        Set(value As Double)
            SetProperty(_FluxOffset, value)
            OnFluxOffsetChanged()
        End Set
    End Property
    Private Sub OnFluxOffsetChanged()
        If EnableFluxLimit Then
            If OnlineUser.Flux > FluxLimit * 1000000000 Then
                FluxFill = Brushes.Red
                Return
            End If
        End If
        Dim b As IBrush = Nothing
        If Program.Selector.SelectedTheme.Style.TryGetResource("ThemeAccentBrush", b) Then
            FluxFill = b
        End If
    End Sub

    Private _FluxFill As IBrush
    Public Property FluxFill As IBrush
        Get
            Return _FluxFill
        End Get
        Set(value As IBrush)
            SetProperty(_FluxFill, value)
        End Set
    End Property

    Public ReadOnly Property ShowConnectionCommand As New ShowDialogCommand(Of ConnectionWindow)(Me)
    Public ReadOnly Property ShowDetailCommand As New ShowDialogCommand(Of DetailWindow)(Me)
    Public ReadOnly Property ShowSettingsCommand As New ShowDialogCommand(Of SettingsWindow)(Me)
End Class
