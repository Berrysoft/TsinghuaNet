Imports System.IO
Imports System.Text
Imports Eto.Forms
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports TsinghuaNet.Helper

Public Class MainViewModel
    Inherits NetViewModel

    Public Sub New()
        MyBase.New()
        Status = New NetPingStatus()
        timer = New UITimer(AddressOf OnlineTimerTick)
        timer.Interval = 1
        If AutoLogin Then
            Login()
        Else
            Refresh()
        End If
    End Sub

    Private Const settingsFilename As String = "settings.json"
    Private ReadOnly Property SettingsPath As String
        Get
            Return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", "TsinghuaNet.Eto", settingsFilename)
        End Get
    End Property

    Private Shared Function GetSettings(json As JObject, key As String, def As JToken) As JToken
        If json.ContainsKey(key) Then
            Return json(key)
        Else
            Return def
        End If
    End Function

    Public Overrides Sub LoadSettings()
        If File.Exists(SettingsPath) Then
            Using stream As New StreamReader(SettingsPath)
                Using reader As New JsonTextReader(stream)
                    Dim json = JObject.Load(reader)
                    Credential.Username = GetSettings(json, "username", String.Empty)
                    Credential.Password = Encoding.UTF8.GetString(Convert.FromBase64String(GetSettings(json, "password", String.Empty)))
                    Credential.State = CInt(GetSettings(json, "state", NetState.Unknown))
                    AutoLogin = CBool(GetSettings(json, "autoLogin", True))
                    UseTimer = CBool(GetSettings(json, "useTimer", True))
                    EnableFluxLimit = CBool(GetSettings(json, "enableFluxLimit", True))
                    FluxLimit = CDbl(GetSettings(json, "fluxLimit", 20.0))
                End Using
            End Using
        End If
    End Sub

    Private Sub CreateSettingsFolder()
        Dim home As New DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))
        home.CreateSubdirectory(Path.Combine(".config", "TsinghuaNet.Eto"))
    End Sub

    Public Overrides Sub SaveSettings()
        Dim json As New JObject
        json("username") = If(Credential.Username, String.Empty)
        json("password") = Convert.ToBase64String(Encoding.UTF8.GetBytes(If(Credential.Password, String.Empty)))
        json("state") = Credential.State
        json("autoLogin") = AutoLogin
        json("useTimer") = UseTimer
        json("enableFluxLimit") = EnableFluxLimit
        json("fluxLimit") = FluxLimit
        CreateSettingsFolder()
        Using stream As New StreamWriter(SettingsPath)
            Using writer As New JsonTextWriter(stream)
                json.WriteTo(writer)
            End Using
        End Using
    End Sub

    Protected Overrides Sub OnSuggestStateChanged()
        MyBase.OnSuggestStateChanged()
        Credential.State = SuggestState
    End Sub

    Private _UseTimer As Boolean
    Public Property UseTimer As Boolean
        Get
            Return _UseTimer
        End Get
        Set(value As Boolean)
            SetProperty(_UseTimer, value)
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

    Private timer As UITimer
    Private Sub OnlineTimerTick()
        OnlineTime += TimeSpan.FromSeconds(1)
    End Sub

    Protected Overrides Async Function RefreshAsync(helper As IConnect) As Task(Of LogResponse)
        Dim res = Await MyBase.RefreshAsync(helper)
        timer.Stop()
        OnlineTime = OnlineUser.OnlineTime
        If UseTimer AndAlso Not String.IsNullOrEmpty(OnlineUser.Username) Then
            timer.Start()
        End If
        If EnableFluxLimit AndAlso OnlineUser.Flux > (FluxLimit * 1000000000) Then
            res = New LogResponse(False, $"流量已使用超过{FluxLimit}GB")
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
