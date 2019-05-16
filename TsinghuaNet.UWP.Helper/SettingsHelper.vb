Imports Windows.Storage

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

Public Module SettingsHelper
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

    Private Const StoredUsernameKey As String = "Username"
    Private Const AutoLoginKey As String = "AutoLogin"
    Private Const BackgroundAutoLoginKey As String = "BackgroundAutoLogin"
    Private Const BackgroundLiveTileKey As String = "BackgroundLiveTile"
    Private Const ThemeKey As String = "Theme"
    Private Const ContentTypeKey As String = "UserContentType"
    Private Const FluxLimitKey As String = "FluxLimit"

    Sub New()
        values = ApplicationData.Current.LocalSettings.Values
        StoredUsername = GetValue(Of String)(StoredUsernameKey)
        AutoLogin = GetValue(AutoLoginKey, True)
        BackgroundAutoLogin = GetValue(BackgroundAutoLoginKey, True)
        BackgroundLiveTile = GetValue(BackgroundLiveTileKey, True)
        Theme = GetValue(Of Integer)(ThemeKey, UserTheme.Default)
        ContentType = GetValue(Of Integer)(ContentTypeKey, UserContentType.Ring)
        FluxLimit = GetValue(Of Long?)(FluxLimitKey, Nothing)
    End Sub

    Public Sub SaveSettings()
        SetValue(StoredUsernameKey, StoredUsername)
        SetValue(AutoLoginKey, AutoLogin)
        SetValue(BackgroundAutoLoginKey, BackgroundAutoLogin)
        SetValue(BackgroundLiveTileKey, BackgroundLiveTile)
        SetValue(Of Integer)(ThemeKey, Theme)
        SetValue(Of Integer)(ContentTypeKey, ContentType)
        SetValue(FluxLimitKey, FluxLimit)
    End Sub

    Public Property StoredUsername As String

    Public Property AutoLogin As Boolean

    Public Property BackgroundAutoLogin As Boolean

    Public Property BackgroundLiveTile As Boolean

    Public Property Theme As UserTheme

    Public Property ContentType As UserContentType

    Public Property FluxLimit As Long?
End Module
