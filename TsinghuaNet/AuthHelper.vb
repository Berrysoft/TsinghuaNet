Imports System.Net.Http
Imports Newtonsoft.Json.Linq

Public MustInherit Class AuthHelper
    Inherits NetHelperBase
    Implements IConnect

    Private Const LogUri = "https://auth{0}.tsinghua.edu.cn/cgi-bin/srun_portal"
    Private Const FluxUri = "https://auth{0}.tsinghua.edu.cn/rad_user_info.php"
    Private Const ChallengeUri = "https://auth{0}.tsinghua.edu.cn/cgi-bin/get_challenge?username={1}&double_stack=1&ip&callback=callback"
    Private ReadOnly AcIds() As Integer = {1, 25, 33, 35, 37}
    Private ReadOnly version As Integer

    Public Sub New(username As String, password As String, version As Integer)
        Me.New(username, password, Nothing, version)
    End Sub

    Public Sub New(username As String, password As String, client As HttpClient, version As Integer)
        MyBase.New(username, password, client)
        Me.version = version
    End Sub

    Private Async Function LogAsync(f As Func(Of Integer, Task(Of Dictionary(Of String, String)))) As Task(Of LogResponse)
        Dim response As LogResponse = Nothing
        Dim uri = String.Format(LogUri, version)
        For Each ac_id In AcIds
            response = LogResponse.ParseFromAuth(Await PostAsync(uri, Await f(ac_id)))
            If response.Succeed Then
                Exit For
            End If
        Next
        Return response
    End Function

    Public Function LoginAsync() As Task(Of LogResponse) Implements IConnect.LoginAsync
        Return LogAsync(AddressOf GetLoginDataAsync)
    End Function

    Public Function LogoutAsync() As Task(Of LogResponse) Implements IConnect.LogoutAsync
        Return LogAsync(AddressOf GetLogoutDataAsync)
    End Function

    Public Async Function GetFluxAsync() As Task(Of FluxUser) Implements IConnect.GetFluxAsync
        Return FluxUser.Parse(Await PostAsync(String.Format(FluxUri, version)))
    End Function

    Private Async Function GetChallengeAsync() As Task(Of String)
        Dim result = Await GetAsync(String.Format(ChallengeUri, version, Username))
        Dim json = JObject.Parse(result.Substring(9, result.Length - 10))
        Return json("challenge")
    End Function

    Private Const LoginInfoJson = "{{""username"": ""{0}"", ""password"": ""{1}"", ""ip"": """", ""acid"": ""{2}"", ""enc_ver"": ""srun_bx1""}}"
    Private Const ChkSumData = "{0}{1}{0}{2}{0}{4}{0}{0}200{0}1{0}{3}"
    Private Async Function GetLoginDataAsync(ac_id As Integer) As Task(Of Dictionary(Of String, String))
        Dim token = Await GetChallengeAsync()
        Dim passwordMD5 = GetHMACMD5(token)
        Dim info = "{SRBX1}" & Base64Encode(XEncode(String.Format(LoginInfoJson, Username, Password, ac_id), token))
        Return New Dictionary(Of String, String) From
        {
            {"action", "login"},
            {"ac_id", ac_id},
            {"double_stack", "1"},
            {"n", "200"},
            {"type", "1"},
            {"username", Username},
            {"password", "{MD5}" & passwordMD5},
            {"info", info},
            {"chksum", GetSHA1(String.Format(ChkSumData, token, Username, passwordMD5, info, ac_id))},
            {"callback", "callback"}
        }
    End Function

    Private Const LogoutInfoJson = "{{""username"": ""{0}"", ""ip"": """", ""acid"": ""{1}"", ""enc_ver"": ""srun_bx1""}}"
    Private Const LogoutChkSumData = "{0}{1}{0}{3}{0}{0}200{0}1{0}{2}"
    Private Async Function GetLogoutDataAsync(ac_id As Integer) As Task(Of Dictionary(Of String, String))
        Dim token = Await GetChallengeAsync()
        Dim info = "{SRBX1}" & Base64Encode(XEncode(String.Format(LogoutInfoJson, Username, ac_id), token))
        Return New Dictionary(Of String, String) From
        {
            {"action", "logout"},
            {"ac_id", ac_id},
            {"double_stack", "1"},
            {"n", "200"},
            {"type", "1"},
            {"username", Username},
            {"info", info},
            {"chksum", GetSHA1(String.Format(LogoutChkSumData, token, Username, info, ac_id))},
            {"callback", "callback"}
        }
    End Function
End Class

Public Class Auth4Helper
    Inherits AuthHelper

    Public Sub New(username As String, password As String)
        MyBase.New(username, password, 4)
    End Sub

    Public Sub New(username As String, password As String, client As HttpClient)
        MyBase.New(username, password, client, 4)
    End Sub
End Class

Public Class Auth6Helper
    Inherits AuthHelper

    Public Sub New(username As String, password As String)
        MyBase.New(username, password, 6)
    End Sub

    Public Sub New(username As String, password As String, client As HttpClient)
        MyBase.New(username, password, client, 6)
    End Sub
End Class
