Imports Windows.Networking.Connectivity
Imports TsinghuaNet.Helper

Public Class InternetStatus
    Inherits NetMapStatus

    Private Shared Function GetInternetStatus() As (Status As NetStatus, Ssid As String)
        Dim profile = NetworkInformation.GetInternetConnectionProfile()
        If profile Is Nothing Then
            Return (NetStatus.Unknown, Nothing)
        End If
        Dim cl = profile.GetNetworkConnectivityLevel()
        If cl = NetworkConnectivityLevel.None Then
            Return (NetStatus.Unknown, Nothing)
        End If
        If profile.IsWwanConnectionProfile Then
            Return (NetStatus.Wwan, Nothing)
        ElseIf profile.IsWlanConnectionProfile Then
            Return (NetStatus.Wlan, profile.WlanConnectionProfileDetails.GetConnectedSsid())
        Else
            Return (NetStatus.Lan, Nothing)
        End If
    End Function

    Public Overrides Function RefreshAsync() As Task
        Dim t = GetInternetStatus()
        Status = t.Status
        Ssid = t.Ssid
        Return Task.CompletedTask
    End Function
End Class
