Option Compare Text

Imports CommandLine

Module Program
    Function Main(args As String()) As Integer
        Return Parser.Default.ParseArguments(Of LoginVerb, LogoutVerb, StatusVerb)(args).MapResult(
            Function(opts As LoginVerb) RunLogin(opts).GetAwaiter().GetResult(),
            Function(opts As LogoutVerb) RunLogout(opts).GetAwaiter().GetResult(),
            Function(opts As StatusVerb) RunStatus(opts).GetAwaiter().GetResult(),
            Function(errs) 1)
    End Function

    Async Function RunLogin(opts As LoginVerb) As Task(Of Integer)
        Dim helper = GetHelper(opts)
        If helper IsNot Nothing Then
            Using helper
                Try
                    Dim res = Await helper.LoginAsync()
                    Console.WriteLine(res.Message)
                Catch ex As Exception
                    Console.WriteLine(ex.Message)
                End Try
            End Using
        End If
        Return 0
    End Function

    Async Function RunLogout(opts As LogoutVerb) As Task(Of Integer)
        Dim helper = GetHelper(opts)
        If helper IsNot Nothing Then
            Using helper
                Try
                    Dim res = Await helper.LogoutAsync()
                    Console.WriteLine(res.Message)
                Catch ex As Exception
                    Console.WriteLine(ex.Message)
                End Try
            End Using
        End If
        Return 0
    End Function

    Async Function RunStatus(opts As StatusVerb) As Task(Of Integer)
        Dim helper = GetHelper(opts)
        If helper IsNot Nothing Then
            Using helper
                Try
                    Dim flux = Await helper.GetFluxAsync()
                    Console.WriteLine("用户：{0}", flux.Username)
                    Console.WriteLine("流量：{0}", StringHelper.GetFluxString(flux.Flux))
                    Console.WriteLine("时长：{0}", flux.OnlineTime)
                    Console.WriteLine("流量：{0}", StringHelper.GetCurrencyString(flux.Balance))
                Catch ex As Exception
                    Console.WriteLine(ex.Message)
                End Try
            End Using
        End If
        Return 0
    End Function

    Function GetHelper(opts As VerbBase) As IConnect
        Return ConnectHelper.GetHelper(GetHost(opts.Host), opts.Username, opts.Password)
    End Function

    Function GetHost(host As String) As NetState
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
    Public MustOverride Property Username As String
    Public MustOverride Property Password As String
End Class

<Verb("login", HelpText:="登录")>
Class LoginVerb
    Inherits VerbBase
    <[Option]("u"c, "username", Required:=True, HelpText:="用户名")>
    Public Overrides Property Username As String
    <[Option]("p"c, "password", Required:=True, HelpText:="密码")>
    Public Overrides Property Password As String
End Class

Class NotRequiredVerbBase
    Inherits VerbBase
    <[Option]("u"c, "username", Required:=False, HelpText:="用户名")>
    Public Overrides Property Username As String
    <[Option]("p"c, "password", Required:=False, HelpText:="密码")>
    Public Overrides Property Password As String
End Class

<Verb("logout", HelpText:="注销")>
Class LogoutVerb
    Inherits NotRequiredVerbBase
End Class

<Verb("status", HelpText:="查看在线状态")>
Class StatusVerb
    Inherits NotRequiredVerbBase
End Class
