Imports Windows.Networking.Connectivity

Class NetworkListener
    Public Event NetworkStatusChanged As NetworkStatusChangedEventHandler

    Protected Sub OnNetworkStatusChanged(sender As Object)
        RaiseEvent NetworkStatusChanged(sender)
    End Sub

    Public Sub New()
        AddHandler NetworkInformation.NetworkStatusChanged, AddressOf OnNetworkStatusChanged
    End Sub

    Protected Overrides Sub Finalize()
        RemoveHandler NetworkInformation.NetworkStatusChanged, AddressOf OnNetworkStatusChanged
    End Sub
End Class
