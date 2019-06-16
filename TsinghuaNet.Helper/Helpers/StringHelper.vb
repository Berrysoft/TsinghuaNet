Imports System.Globalization

Public Module StringHelper
    Public Function GetCurrencyString(currency As Decimal) As String
        Static zhCulture As New CultureInfo("zh-CN")
        Return currency.ToString("C2", zhCulture)
    End Function

    Public Function GetNetStateString(state As NetState) As String
        Select Case state
            Case NetState.Net
                Return "Net"
            Case NetState.Auth4
                Return "Auth4"
            Case NetState.Auth6
                Return "Auth6"
            Case Else
                Return "不登录"
        End Select
    End Function

    Public Function GetNetStatusString(status As NetStatus) As String
        Select Case status
            Case NetStatus.Wwan
                Return "移动流量"
            Case NetStatus.Wlan
                Return "无线网络"
            Case NetStatus.Lan
                Return "有线网络"
            Case Else
                Return "未连接"
        End Select
    End Function

    Public Function GetNetStatusString(status As NetStatus, ssid As String) As String
        If String.IsNullOrEmpty(ssid) Then
            Return GetNetStatusString(status)
        Else
            Return $"{GetNetStatusString(status)} - {ssid}"
        End If
    End Function
End Module
