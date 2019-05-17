Imports System.Net
Imports System.Runtime.CompilerServices
Imports CommandLine
Imports CommandLine.Text
Imports TsinghuaNet.Helper

Module VerbHelper
    Public Status As New NetPingStatus

    <Extension>
    Public Async Function GetHelperAsync(opts As NetVerbBase) As Task(Of IConnect)
        If opts.Host = OptionNetState.Auto Then
            opts.Host = Await Status.SuggestAsync()
        End If
        Dim v = TryCast(opts, IConnectVerb)
        If v IsNot Nothing Then
            Return ConnectHelper.GetHelper(opts.Host, v.Username, v.Password)
        Else
            Return ConnectHelper.GetHelper(opts.Host)
        End If
    End Function
End Module

Enum OptionNetState
    Auto
    Net
    Auth4
    Auth6
End Enum

Enum OptionNetDetailOrder
    Login = 0
    LoginTime = Login
    Logout = 2
    LogoutTime = Logout
    Flux = 4
End Enum

MustInherit Class VerbBase
    Protected Const DateTimeFormat As String = "yyyy-M-d h:mm:ss"
    Protected Const DateFormat As String = "yyyy-M-d"

    Public MustOverride Function RunAsync() As Task
End Class

MustInherit Class NetVerbBase
    Inherits VerbBase
    <[Option]("s"c, "host", Required:=False, [Default]:=OptionNetState.Auto, HelpText:="连接方式：[auto], net, auth4, auth6")>
    Public Property Host As OptionNetState
End Class

Interface IConnectVerb
    Property Username As String
    Property Password As String
End Interface

MustInherit Class RequiredVerbBase
    Inherits NetVerbBase
    Implements IConnectVerb
    <[Option]("u"c, "username", Required:=True, HelpText:="用户名")>
    Public Property Username As String Implements IConnectVerb.Username
    <[Option]("p"c, "password", Required:=True, HelpText:="密码")>
    Public Property Password As String Implements IConnectVerb.Password
End Class

MustInherit Class NotRequiredVerbBase
    Inherits NetVerbBase
    Implements IConnectVerb
    <[Option]("u"c, "username", Required:=False, HelpText:="用户名")>
    Public Property Username As String Implements IConnectVerb.Username
    <[Option]("p"c, "password", Required:=False, HelpText:="密码")>
    Public Property Password As String Implements IConnectVerb.Password
End Class

MustInherit Class UseregVerbBase
    Inherits VerbBase
    Implements IConnectVerb
    <[Option]("u"c, "username", Required:=True, HelpText:="用户名")>
    Public Property Username As String Implements IConnectVerb.Username
    <[Option]("p"c, "password", Required:=True, HelpText:="密码")>
    Public Property Password As String Implements IConnectVerb.Password
End Class

<Verb("login", HelpText:="登录")>
Class LoginVerb
    Inherits RequiredVerbBase

    <Usage()>
    Public Shared ReadOnly Iterator Property Examples As IEnumerable(Of Example)
        Get
            Yield New Example("使用默认（自动判断）方式登录", New LoginVerb() With {.Username = "用户名", .Password = "密码"})
            Yield New Example("使用auth4方式登录", New LoginVerb() With {.Host = OptionNetState.Auth4, .Username = "用户名", .Password = "密码"})
        End Get
    End Property

    Public Overrides Async Function RunAsync() As Task
        Using helper = Await GetHelperAsync()
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

    <Usage()>
    Public Shared ReadOnly Iterator Property Examples As IEnumerable(Of Example)
        Get
            Yield New Example("使用默认（自动判断）方式注销，不需要用户名密码", New LogoutVerb())
            Yield New Example("使用auth4方式注销，需要用户名密码", New LogoutVerb() With {.Host = OptionNetState.Auth4, .Username = "用户名", .Password = "密码"})
        End Get
    End Property

    Public Overrides Async Function RunAsync() As Task
        Using helper = Await GetHelperAsync()
            If helper IsNot Nothing Then
                Dim res = Await helper.LogoutAsync()
                Console.WriteLine(res.Message)
            End If
        End Using
    End Function
End Class

<Verb("status", HelpText:="查看在线状态")>
Class StatusVerb
    Inherits NetVerbBase

    <Usage()>
    Public Shared ReadOnly Iterator Property Examples As IEnumerable(Of Example)
        Get
            Yield New Example("使用默认（自动判断）方式", New StatusVerb())
            Yield New Example("使用auth4方式", New StatusVerb() With {.Host = OptionNetState.Auth4})
        End Get
    End Property

    Public Overrides Async Function RunAsync() As Task
        Using helper = Await GetHelperAsync()
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
    Inherits UseregVerbBase

    <Usage()>
    Public Shared ReadOnly Iterator Property Examples As IEnumerable(Of Example)
        Get
            Yield New Example("查询", New OnlineVerb() With {.Username = "用户名", .Password = "密码"})
        End Get
    End Property

    Public Overrides Async Function RunAsync() As Task
        Using helper As New UseregHelper(Username, Password)
            Dim res = Await helper.LoginAsync()
            If res.Succeed Then
                Dim users = Await helper.GetUsersAsync()
                Console.WriteLine("|       IP       |       登录时间       |   客户端   |")
                Console.WriteLine(New String("="c, 54))
                For Each user In users
                    Console.WriteLine("| {0}| {1}| {2}|", user.Address.ToString().PadRight(15), user.LoginTime.ToString(DateTimeFormat).PadRight(21), user.Client.PadRight(11))
                Next
            Else
                Console.WriteLine(res.Message)
            End If
        End Using
    End Function
End Class

<Verb("drop", HelpText:="下线IP")>
Class DropVerb
    Inherits UseregVerbBase

    <Usage()>
    Public Shared ReadOnly Iterator Property Examples As IEnumerable(Of Example)
        Get
            Yield New Example("下线一个IP", New DropVerb() With {.Username = "用户名", .Password = "密码", .Address = "IP地址"})
        End Get
    End Property

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
    Inherits UseregVerbBase

    <Usage()>
    Public Shared ReadOnly Iterator Property Examples As IEnumerable(Of Example)
        Get
            Yield New Example("使用默认排序（注销时间，升序）查询明细", New DetailVerb() With {.Username = "用户名", .Password = "密码"})
            Yield New Example("使用登陆时间（升序）查询明细", New DetailVerb() With {.Username = "用户名", .Password = "密码", .Order = OptionNetDetailOrder.Login})
            Yield New Example("使用流量降序查询明细", New DetailVerb() With {.Username = "用户名", .Password = "密码", .Order = OptionNetDetailOrder.Flux, .Descending = True})
            Yield New Example("使用流量降序查询明细，并按注销日期组合", New DetailVerb() With {.Username = "用户名", .Password = "密码", .Order = OptionNetDetailOrder.Flux, .Descending = True, .Grouping = True})
        End Get
    End Property

    <[Option]("o"c, "order", Required:=False, [Default]:=OptionNetDetailOrder.Logout, HelpText:="排序指标：[logout<time>], login<time>, flux")>
    Public Property Order As OptionNetDetailOrder
    <[Option]("d"c, "descending", Required:=False, HelpText:="降序")>
    Public Property Descending As Boolean
    <[Option]("g"c, "grouping", Required:=False, HelpText:="按注销日期组合")>
    Public Property Grouping As Boolean

    Public Overrides Async Function RunAsync() As Task
        Using helper As New UseregHelper(Username, Password)
            Dim res = Await helper.LoginAsync()
            If res.Succeed Then
                If Grouping Then
                    Dim details = Await helper.GetDetailsAsync()
                    Dim now = Date.Now
                    Console.WriteLine("|    日期    |    流量    |")
                    Console.WriteLine(New String("="c, 27))
                    Dim query = From d In details Group By d.LogoutTime.Day Into TotalFlux = Sum(d.Flux)
                    Dim orderedQuery = If(Order = NetDetailOrder.Flux, query.OrderBy(Function(d) d.TotalFlux, Descending), query.OrderBy(Function(d) d.Day, Descending))
                    For Each p In orderedQuery
                        Console.WriteLine("| {0}|{1} |", New Date(now.Year, now.Month, p.Day).ToString(DateFormat).PadRight(11), StringHelper.GetFluxString(p.TotalFlux).PadLeft(11))
                    Next
                Else
                    Dim details = Await helper.GetDetailsAsync(Order + If(Descending, 1, 0))
                    Console.WriteLine("|       登录时间       |       注销时间       |    流量    |")
                    Console.WriteLine(New String("="c, 60))
                    For Each d In details
                        Console.WriteLine("| {0}| {1}|{2} |", d.LoginTime.ToString(DateTimeFormat).PadRight(21), d.LogoutTime.ToString(DateTimeFormat).PadRight(21), StringHelper.GetFluxString(d.Flux).PadLeft(11))
                    Next
                End If
            Else
                Console.WriteLine(res.Message)
            End If
        End Using
    End Function
End Class

<Verb("suggestion", HelpText:="获取建议的连接方式")>
Class SuggestionVerb
    Inherits VerbBase

    <Usage()>
    Public Shared ReadOnly Iterator Property Examples As IEnumerable(Of Example)
        Get
            Yield New Example("获取建议", New SuggestionVerb())
        End Get
    End Property

    Public Overrides Async Function RunAsync() As Task
        Console.WriteLine(StringHelper.GetNetStateString(Await VerbHelper.Status.SuggestAsync()))
    End Function
End Class
