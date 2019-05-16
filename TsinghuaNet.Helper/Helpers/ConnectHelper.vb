Imports System.Net.Http

Public Enum NetState
    Unknown
    Net
    Auth4
    Auth6
End Enum

Public Module ConnectHelper
    Private ReadOnly StateHelperMap As New Dictionary(Of NetState, Type) From
    {
        {NetState.Net, GetType(NetHelper)},
        {NetState.Auth4, GetType(Auth4Helper)},
        {NetState.Auth6, GetType(Auth6Helper)}
    }

    Private Function GetHelperImpl(state As NetState, ParamArray args As Object()) As IConnect
        If StateHelperMap.ContainsKey(state) Then
            Return Activator.CreateInstance(StateHelperMap(state), args)
        Else
            Return Nothing
        End If
    End Function

    Public Function GetHelper(state As NetState) As IConnect
        Return GetHelperImpl(state)
    End Function

    Public Function GetHelper(state As NetState, client As HttpClient) As IConnect
        Return GetHelperImpl(state, client)
    End Function

    Public Function GetHelper(state As NetState, username As String, password As String) As IConnect
        Return GetHelperImpl(state, username, password)
    End Function

    Public Function GetHelper(state As NetState, username As String, password As String, client As HttpClient) As IConnect
        Return GetHelperImpl(state, username, password, client)
    End Function
End Module
