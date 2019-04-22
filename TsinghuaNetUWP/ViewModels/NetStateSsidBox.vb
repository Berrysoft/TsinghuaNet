Imports TsinghuaNetUWP.Helper

Public Class NetStateSsidBox
    Public Sub New(ssid As String, value As NetState)
        Me.Ssid = ssid
        Me.Value = value
    End Sub

    Public Property Ssid As String
    Public Property Value As NetState
End Class
