Imports Berrysoft.Tsinghua.Net

Public Module ConnectHelper
    Public Function GetHelper(state As NetState) As IConnect
        Select Case state
            Case NetState.Net
                Return New NetHelper()
            Case NetState.Auth4
                Return New Auth4Helper()
            Case NetState.Auth6
                Return New Auth6Helper()
            Case Else
                Return Nothing
        End Select
    End Function

    Public Function GetHelper(state As NetState, username As String, password As String) As IConnect
        Select Case state
            Case NetState.Net
                Return New NetHelper(username, password)
            Case NetState.Auth4
                Return New Auth4Helper(username, password)
            Case NetState.Auth6
                Return New Auth6Helper(username, password)
            Case Else
                Return Nothing
        End Select
    End Function
End Module
