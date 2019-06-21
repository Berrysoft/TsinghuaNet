Imports System.Net.Http

Public Enum NetState
    Unknown
    Net
    Auth4
    Auth6
End Enum

Public Module ConnectHelper
    Public Function GetHelper(state As NetState) As IConnect
        Return GetHelper(state, Nothing, Nothing, Nothing)
    End Function

    Public Function GetHelper(state As NetState, client As HttpClient) As IConnect
        Return GetHelper(state, Nothing, Nothing, client)
    End Function

    Public Function GetHelper(state As NetState, username As String, password As String) As IConnect
        Return GetHelper(state, username, password, Nothing)
    End Function

    Public Function GetHelper(state As NetState, username As String, password As String, client As HttpClient) As IConnect
        Select Case state
            Case NetState.Net
                Return New NetHelper(username, password, client)
            Case NetState.Auth4
                Return New Auth4Helper(username, password, client)
            Case NetState.Auth6
                Return New Auth6Helper(username, password, client)
            Case Else
                Return Nothing
        End Select
    End Function
End Module
