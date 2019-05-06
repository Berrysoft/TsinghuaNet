Imports TsinghuaNet.Helper

Public Module UserHelper
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
