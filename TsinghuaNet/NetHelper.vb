Imports System.Net.Http

Public Class NetHelper
    Inherits NetHelperBase
    Implements IConnect

    Private Const LogUri = "http://net.tsinghua.edu.cn/do_login.php"
    Private Const FluxUri = "http://net.tsinghua.edu.cn/rad_user_info.php"
    Private Const LogoutData = "action=logout"

    Public Sub New(username As String, password As String)
        MyBase.New(username, password)
    End Sub

    Public Sub New(username As String, password As String, client As HttpClient)
        MyBase.New(username, password, client)
    End Sub

    Public Async Function LoginAsync() As Task(Of LogResponse) Implements IConnect.LoginAsync
        Return LogResponse.ParseFromNet(Await PostAsync(LogUri, New Dictionary(Of String, String) From
        {
            {"action", "login"},
            {"ac_id", "1"},
            {"username", Username},
            {"password", "{MD5_HEX}" & GetMD5(Password)}
        }))
    End Function

    Public Async Function LogoutAsync() As Task(Of LogResponse) Implements IConnect.LogoutAsync
        Return LogResponse.ParseFromNet(Await PostAsync(LogUri, LogoutData))
    End Function

    Public Async Function GetFluxAsync() As Task(Of FluxUser) Implements IConnect.GetFluxAsync
        Return FluxUser.Parse(Await PostAsync(FluxUri))
    End Function
End Class
