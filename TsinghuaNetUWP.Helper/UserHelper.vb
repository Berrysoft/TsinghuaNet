Imports System.Globalization

Public Module UserHelper
    Public Function GetFluxString(flux As Double) As String
        If flux < 1000 Then
            Return $"{flux} B"
        End If
        flux /= 1000
        If flux < 1000 Then
            Return $"{flux:F2} KB"
        End If
        flux /= 1000
        If flux < 1000 Then
            Return $"{flux:F2} MB"
        End If
        flux /= 1000
        Return $"{flux:F2} GB"
    End Function

    Public Function GetCurrencyString(currency As Decimal) As String
        Static culture As New CultureInfo("zh-CN")
        Return currency.ToString("C2", culture)
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
                Return "不需要登录"
        End Select
    End Function

    Public Function GetInternetStatusString(status As InternetStatus) As String
        Select Case status
            Case InternetStatus.Wwan
                Return "移动流量"
            Case InternetStatus.Wlan
                Return "无线网络"
            Case InternetStatus.Lan
                Return "有线网络"
            Case Else
                Return "未连接"
        End Select
    End Function

    Public Function GetInternetStatusString(status As InternetStatus, ssid As String) As String
        If String.IsNullOrEmpty(ssid) Then
            Return GetInternetStatusString(status)
        Else
            Return $"{GetInternetStatusString(status)} - {ssid}"
        End If
    End Function

    Public Const BaseFlux As Long = 25000000000

    Public Function GetMaxFlux(flux As Long, balance As Decimal) As Long
        Return Math.Max(flux, BaseFlux) + balance / 2 * 1000 * 1000 * 1000
    End Function
End Module
