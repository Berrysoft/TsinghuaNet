Imports TsinghuaNetUWP.Helper

Public NotInheritable Class NetStateSsidBox
    Public Sub New(ssid As String, value As NetState)
        Me.Ssid = ssid
        Me.Value = value
    End Sub

    Public Property Ssid As String
    Public Property Value As NetState
End Class

Module NetStateHelper
    Public ReadOnly NetStateNameList As New List(Of String) From {"不登录", "Net", "Auth4", "Auth6"}
End Module
