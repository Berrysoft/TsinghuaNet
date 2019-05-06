Imports System.Net.NetworkInformation

Public Module SuggestionHelper
    Private Async Function CanConnectTo(uri As String) As Task(Of Boolean)
        Try
            Dim p As New Ping
            Dim reply = Await p.SendPingAsync(uri)
            Return reply.Status = IPStatus.Success
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Async Function GetSuggestion() As Task(Of NetState)
        If Await CanConnectTo("auth4.tsinghua.edu.cn") Then
            Return NetState.Auth4
        ElseIf Await CanConnectTo("net.tsinghua.edu.cn") Then
            Return NetState.Net
        Else
            Return NetState.Unknown
        End If
    End Function
End Module
