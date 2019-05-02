Option Compare Text

Imports System.Net
Imports System.Runtime.CompilerServices
Imports System.Text
Imports CommandLine

Module VerbHelper
    <Extension>
    Public Function GetHelper(opts As VerbBase) As IConnect
        Dim v = TryCast(opts, IConnectVerb)
        If v IsNot Nothing Then
            Return ConnectHelper.GetHelper(GetHost(opts.Host), v.Username, v.Password)
        Else
            Return ConnectHelper.GetHelper(GetHost(opts.Host))
        End If
    End Function

    Private Function GetHost(host As String) As NetState
        Select Case host
            Case "auth4"
                Return NetState.Auth4
            Case "auth6"
                Return NetState.Auth6
            Case Else
                Return NetState.Net
        End Select
    End Function
End Module

MustInherit Class VerbBase
    <[Option]("s"c, "host", Required:=False, HelpText:="连接方式")>
    Public Property Host As String

    Public MustOverride Function RunAsync() As Task
End Class

Interface IConnectVerb
    Property Username As String
    Property Password As String
End Interface

MustInherit Class RequiredVerbBase
    Inherits VerbBase
    Implements IConnectVerb
    <[Option]("u"c, "username", Required:=True, HelpText:="用户名")>
    Public Property Username As String Implements IConnectVerb.Username
    <[Option]("p"c, "password", Required:=True, HelpText:="密码")>
    Public Property Password As String Implements IConnectVerb.Password
End Class

MustInherit Class NotRequiredVerbBase
    Inherits VerbBase
    Implements IConnectVerb
    <[Option]("u"c, "username", Required:=False, HelpText:="用户名")>
    Public Property Username As String Implements IConnectVerb.Username
    <[Option]("p"c, "password", Required:=False, HelpText:="密码")>
    Public Property Password As String Implements IConnectVerb.Password
End Class

<Verb("login", HelpText:="登录")>
Class LoginVerb
    Inherits RequiredVerbBase

    Public Overrides Async Function RunAsync() As Task
        Using helper = GetHelper()
            If helper IsNot Nothing Then
                Dim res = Await helper.LoginAsync()
                Console.WriteLine(res.Message)
            End If
        End Using
    End Function
End Class

<Verb("logout", HelpText:="注销")>
Class LogoutVerb
    Inherits NotRequiredVerbBase

    Public Overrides Async Function RunAsync() As Task
        Using helper = GetHelper()
            If helper IsNot Nothing Then
                Dim res = Await helper.LogoutAsync()
                Console.WriteLine(res.Message)
            End If
        End Using
    End Function
End Class

<Verb("status", HelpText:="查看在线状态")>
Class StatusVerb
    Inherits VerbBase

    Public Overrides Async Function RunAsync() As Task
        Using helper = GetHelper()
            If helper IsNot Nothing Then
                Dim flux = Await helper.GetFluxAsync()
                Console.WriteLine("用户：{0}", flux.Username)
                Console.WriteLine("流量：{0}", StringHelper.GetFluxString(flux.Flux))
                Console.WriteLine("时长：{0}", flux.OnlineTime)
                Console.WriteLine("流量：{0}", StringHelper.GetCurrencyString(flux.Balance))
            End If
        End Using
    End Function
End Class

<Verb("online", HelpText:="查询在线IP")>
Class OnlineVerb
    Inherits RequiredVerbBase

    Public Overrides Async Function RunAsync() As Task
        Using helper As New UseregHelper(Username, Password)
            Dim res = Await helper.LoginAsync()
            If res.Succeed Then
                Dim users = Await helper.GetUsersAsync()
                Console.WriteLine("|       IP       |       登录时间       |   客户端   |")
                Console.WriteLine(New String("="c, 54))
                For Each user In users
                    Console.WriteLine("| {0}| {1}| {2}|", user.Address.ToString().PadRight(15), user.LoginTime.ToString().PadRight(21), user.Client.PadRight(11))
                Next
            Else
                Console.WriteLine(res.Message)
            End If
        End Using
    End Function
End Class

<Verb("drop", HelpText:="下线IP")>
Class DropVerb
    Inherits RequiredVerbBase

    <[Option]("a"c, "address", Required:=True, HelpText:="IP地址")>
    Public Property Address As String

    Public Overrides Async Function RunAsync() As Task
        Dim ip = IPAddress.Parse(Address)
        Using helper As New UseregHelper(Username, Password)
            Dim res = Await helper.LoginAsync()
            If res.Succeed Then
                res = Await helper.LogoutAsync(ip)
                If Not res.Succeed Then
                    Console.WriteLine(res.Message)
                End If
            Else
                Console.WriteLine(res.Message)
            End If
        End Using
    End Function
End Class

<Verb("detail", HelpText:="流量明细")>
Class DetailVerb
    Inherits RequiredVerbBase

    Public Overrides Async Function RunAsync() As Task
        Using helper As New UseregHelper(Username, Password)
            Dim res = Await helper.LoginAsync()
            If res.Succeed Then
                Dim details = Await helper.GetDetailsAsync()
                Console.WriteLine("|       登录时间       |       注销时间       |    流量    |")
                Console.WriteLine(New String("="c, 60))
                For Each d In details
                    Console.WriteLine("| {0}| {1}|{2} |", d.LoginTime.ToString().PadRight(21), d.LogoutTime.ToString().PadRight(21), StringHelper.GetFluxString(d.Flux).PadLeft(11))
                Next
            Else
                Console.WriteLine(res.Message)
            End If
        End Using
    End Function
End Class
