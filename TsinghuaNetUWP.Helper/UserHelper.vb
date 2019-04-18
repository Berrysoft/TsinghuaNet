Imports System.Globalization
Imports Berrysoft.Tsinghua.Net

Public Module UserHelper
    Public Function GetFluxString(flux As Long) As String
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

    Public Function GetTimeSpanString(time As TimeSpan) As String
        Return time.ToString()
    End Function

    Public Function GetCurrencyString(currency As Decimal) As String
        Return currency.ToString("C2", New CultureInfo("zh-CN"))
    End Function

    Public Function GetNetStateString(state As NetState) As String
        Select Case state
            Case NetState.Net
                Return "Net - http://net.tsinghua.edu.cn/"
            Case NetState.Auth4
                Return "Auth4 - http://auth4.tsinghua.edu.cn/"
            Case NetState.Auth6
                Return "Auth6 - http://auth6.tsinghua.edu.cn/"
            Case Else
                Return "不需要登录"
        End Select
    End Function

    Public Function GetNetStateSimpleString(state As NetState) As String
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

    Public Const BaseFlux As Long = 25000000000

    Public Function GetMaxFlux(flux As Long, balance As Decimal) As Long
        Return Math.Max(flux, BaseFlux) + balance / 2 * 1000 * 1000 * 1000
    End Function

    Public Function GetResponseString(response As LogResponse) As String
        Return response.Message
    End Function
End Module
