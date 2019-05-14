Imports Windows.Networking.Connectivity

Public Module InternetStatusHelper
    Public Function InternetAvailable() As Boolean
        Dim profile = NetworkInformation.GetInternetConnectionProfile()
        If profile Is Nothing Then
            Return False
        End If
        Return profile.GetNetworkConnectivityLevel() = NetworkConnectivityLevel.InternetAccess
    End Function

    Public Function GetInternetStatus() As (Status As InternetStatus, Ssid As String)
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
End Module
