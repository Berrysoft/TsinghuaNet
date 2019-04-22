Imports Windows.Data.Json
Imports Windows.Networking.Connectivity
Imports Windows.Storage

Public Enum NetState
    Unknown
    Net
    Auth4
    Auth6
End Enum

Public Enum InternetStatus
    Unknown
    Wwan
    Wlan
    Lan
End Enum

Public Enum UserTheme
    [Default]
    Light
    Dark
    Auto
End Enum

Public Enum UserContentType
    Line
    Ring
    Water
    Graph
End Enum

Public NotInheritable Class SettingsHelper
    Private values As IPropertySet

    Private Function GetValue(Of T)(key As String, Optional def As T = Nothing) As T
        If values.ContainsKey(key) Then
            Return values(key)
        Else
            Return def
        End If
    End Function

    Private Sub SetValue(Of T)(key As String, value As T)
        If values.ContainsKey(key) Then
            values(key) = value
        Else
            values.Add(key, value)
        End If
    End Sub

    Private Shared Function GetMapFromJson(json As JsonObject) As IDictionary(Of String, NetState)
        Dim result As New Dictionary(Of String, NetState)
        For Each pair In json
            result.Add(pair.Key, pair.Value.GetNumber)
        Next
        Return result
    End Function

    Private Shared Function GetJsonFromMap(map As Dictionary(Of String, NetState)) As JsonObject
        Dim result As New JsonObject
        For Each pair In map
            result.Add(pair.Key, JsonValue.CreateNumberValue(pair.Value))
        Next
        Return result
    End Function

    Private Const StoredUsernameKey As String = "Username"
    Private Const AutoLoginKey As String = "AutoLogin"
    Private Const BackgroundAutoLoginKey As String = "BackgroundAutoLogin"
    Private Const BackgroundLiveTileKey As String = "BackgroundLiveTile"
    Private Const LanStateKey As String = "LanState"
    Private Const WwanStateKey As String = "WwanState"
    Private Const WlanStateKey As String = "WlanState"
    Private Const ThemeKey As String = "Theme"
    Private Const ContentTypeKey As String = "UserContentType"

    Public Sub New()
        values = ApplicationData.Current.LocalSettings.Values
        StoredUsername = GetValue(Of String)(StoredUsernameKey)
        AutoLogin = GetValue(AutoLoginKey, True)
        BackgroundAutoLogin = GetValue(BackgroundAutoLoginKey, True)
        BackgroundLiveTile = GetValue(BackgroundLiveTileKey, True)
        LanState = GetValue(Of Integer)(LanStateKey, NetState.Auth4)
        WwanState = GetValue(Of Integer)(WwanStateKey, NetState.Unknown)
        Theme = GetValue(Of Integer)(ThemeKey, UserTheme.Default)
        ContentType = GetValue(Of Integer)(ContentTypeKey, UserContentType.Ring)
        Dim json As String = GetValue(Of String)(WlanStateKey)
        Dim jsonobj As JsonObject = Nothing
        If String.IsNullOrEmpty(json) OrElse (Not JsonObject.TryParse(json, jsonobj)) Then
            WlanStates = DefWlanStates()
        Else
            WlanStates = GetMapFromJson(jsonobj)
        End If
    End Sub

    Public Sub SaveSettings()
        SetValue(StoredUsernameKey, StoredUsername)
        SetValue(AutoLoginKey, AutoLogin)
        SetValue(BackgroundAutoLoginKey, BackgroundAutoLogin)
        SetValue(BackgroundLiveTileKey, BackgroundLiveTile)
        SetValue(Of Integer)(LanStateKey, LanState)
        SetValue(Of Integer)(WwanStateKey, WwanState)
        SetValue(Of Integer)(ThemeKey, Theme)
        SetValue(Of Integer)(ContentTypeKey, ContentType)
        SetValue(WlanStateKey, GetJsonFromMap(WlanStates).ToString())
    End Sub

    Public Property StoredUsername As String

    Public Property AutoLogin As Boolean

    Public Property BackgroundAutoLogin As Boolean

    Public Property BackgroundLiveTile As Boolean

    Public Property LanState As NetState

    Public Property WwanState As NetState

    Public Property WlanStates As Dictionary(Of String, NetState)

    Public ReadOnly Property WlanState(ssid As String) As NetState
        Get
            If WlanStates.ContainsKey(ssid) Then
                Return WlanStates(ssid)
            Else
                Return NetState.Unknown
            End If
        End Get
    End Property

    Public Property Theme As UserTheme

    Public Property ContentType As UserContentType

    Public Function SuggestNetState(status As InternetStatus, ssid As String) As NetState
        Select Case status
            Case InternetStatus.Lan
                Return LanState
            Case InternetStatus.Wwan
                Return WwanState
            Case InternetStatus.Wlan
                Return WlanState(ssid)
            Case Else
                Return NetState.Unknown
        End Select
    End Function

    Public Shared Function DefWlanStates() As IDictionary(Of String, NetState)
        Return New Dictionary(Of String, NetState) From
        {
            {"Tsinghua", NetState.Net},
            {"Tsinghua-5G", NetState.Net},
            {"Tsinghua-IPv4", NetState.Auth4},
            {"Wifi.郑裕彤讲堂", NetState.Net}
        }
    End Function

    Public Shared Function InternetAvailable() As Boolean
        Dim profile = NetworkInformation.GetInternetConnectionProfile()
        If profile Is Nothing Then
            Return False
        End If
        Return profile.GetNetworkConnectivityLevel() = NetworkConnectivityLevel.InternetAccess
    End Function

    Public Shared Function GetInternetStatus() As (Status As InternetStatus, Ssid As String)
        Dim profile = NetworkInformation.GetInternetConnectionProfile()
        If profile Is Nothing Then
            Return (InternetStatus.Unknown, Nothing)
        End If
        Dim cl = profile.GetNetworkConnectivityLevel()
        If cl = NetworkConnectivityLevel.None Then
            Return (InternetStatus.Unknown, Nothing)
        End If
        If profile.IsWwanConnectionProfile Then
            Return (InternetStatus.Wwan, Nothing)
        ElseIf profile.IsWlanConnectionProfile Then
            Return (InternetStatus.Wlan, profile.WlanConnectionProfileDetails.GetConnectedSsid())
        Else
            Return (InternetStatus.Lan, Nothing)
        End If
    End Function
End Class
