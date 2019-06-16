Imports System.Globalization
Imports System.Net
Imports System.Net.Http
Imports HtmlAgilityPack

Public Class NetUser
    Public ReadOnly Property Address As IPAddress
    Public ReadOnly Property LoginTime As Date
    Public ReadOnly Property Client As String

    Public Sub New(address As IPAddress, loginTime As Date, client As String)
        Me.Address = address
        Me.LoginTime = loginTime
        Me.Client = client
    End Sub

    Public Shared Operator =(u1 As NetUser, u2 As NetUser) As Boolean
        If u1 Is Nothing OrElse u2 Is Nothing Then
            Return u1 Is u2
        Else
            Return u1.Address.Equals(u2.Address) AndAlso u1.LoginTime = u2.LoginTime AndAlso u1.Client = u2.Client
        End If
    End Operator

    Public Shared Operator <>(u1 As NetUser, u2 As NetUser) As Boolean
        Return Not u1 = u2
    End Operator

    Public Overrides Function Equals(obj As Object) As Boolean
        Return TypeOf obj Is NetUser AndAlso Me = CType(obj, NetUser)
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return If(Address?.GetHashCode(), 0) Xor LoginTime.GetHashCode() Xor If(Client?.GetHashCode(), 0)
    End Function
End Class

Public Class NetDetail
    Public ReadOnly Property LoginTime As Date
    Public ReadOnly Property LogoutTime As Date
    Public ReadOnly Property Flux As ByteSize

    Public Sub New(login As Date, logout As Date, flux As ByteSize)
        Me.LoginTime = login
        Me.LogoutTime = logout
        Me.Flux = flux
    End Sub

    Public Shared Operator =(d1 As NetDetail, d2 As NetDetail) As Boolean
        If d1 Is Nothing OrElse d2 Is Nothing Then
            Return d1 Is d2
        Else
            Return d1.LoginTime = d2.LoginTime AndAlso d1.LogoutTime = d2.LogoutTime AndAlso d1.Flux = d2.Flux
        End If
    End Operator

    Public Shared Operator <>(d1 As NetDetail, d2 As NetDetail) As Boolean
        Return Not d1 = d2
    End Operator

    Public Overrides Function Equals(obj As Object) As Boolean
        Return TypeOf obj Is NetDetail AndAlso Me = CType(obj, NetDetail)
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return LoginTime.GetHashCode() Xor LogoutTime.GetHashCode() Xor Flux.GetHashCode()
    End Function
End Class

Public Enum NetDetailOrder
    LoginTime
    LogoutTime
    Flux
End Enum

Public Class UseregHelper
    Inherits NetHelperBase
    Implements ILog

    Private Const LogUri = "http://usereg.tsinghua.edu.cn/do.php"
    Private Const InfoUri = "http://usereg.tsinghua.edu.cn/online_user_ipv4.php"
    Private Const DetailUri = "http://usereg.tsinghua.edu.cn/user_detail_list.php?action=query&desc={6}&order={5}&start_time={0}-{1}-01&end_time={0}-{1}-{2}&page={3}&offset={4}"
    Private Const LogoutData = "action=logout"
    Private Const DropData = "action=drop&user_ip={0}"

    Public Sub New(username As String, password As String)
        MyBase.New(username, password)
    End Sub

    Public Sub New(username As String, password As String, client As HttpClient)
        MyBase.New(username, password, client)
    End Sub

    Public Async Function LoginAsync() As Task(Of LogResponse) Implements ILog.LoginAsync
        Return LogResponse.ParseFromUsereg(Await PostAsync(LogUri, New Dictionary(Of String, String) From
        {
            {"action", "login"},
            {"user_login_name", Username},
            {"user_password", GetMD5(Password)}
        }))
    End Function

    Public Async Function LogoutAsync() As Task(Of LogResponse) Implements ILog.LogoutAsync
        Return LogResponse.ParseFromUsereg(Await PostAsync(LogUri, LogoutData))
    End Function

    Public Async Function LogoutAsync(ip As IPAddress) As Task(Of LogResponse)
        Return LogResponse.ParseFromUsereg(Await PostAsync(InfoUri, String.Format(DropData, ip.ToString())))
    End Function

    Private Const DateTimeFormat = "yyyy-MM-dd HH:mm:ss"

    Public Async Function GetUsersAsync() As Task(Of IEnumerable(Of NetUser))
        Dim userhtml = Await GetAsync(InfoUri)
        Dim doc As New HtmlDocument
        doc.LoadHtml(userhtml)
        Return From tr In doc.DocumentNode.Element("html").Element("body").Element("table").Element("tr").Elements("td").Last().Elements("table").ElementAt(1).Elements("tr").Skip(1)
               Let tds = (From td In tr.Elements("td").Skip(1)
                          Select td.FirstChild?.InnerText).ToArray()
               Select New NetUser(
                   IPAddress.Parse(tds(0)),
                   Date.ParseExact(tds(1), DateTimeFormat, CultureInfo.InvariantCulture),
                   tds(10))
    End Function

    Private Shared ReadOnly OrderQueryMap As New Dictionary(Of NetDetailOrder, String) From
    {
        {NetDetailOrder.LoginTime, "user_login_time"},
        {NetDetailOrder.LogoutTime, "user_drop_time"},
        {NetDetailOrder.Flux, "user_in_bytes"}
    }

    Public Async Function GetDetailsAsync(order As NetDetailOrder, descending As Boolean) As Task(Of IEnumerable(Of NetDetail))
        Const offset As Integer = 100
        Dim now As Date = Date.Now
        Dim list As New List(Of NetDetail)()
        Dim i As Integer = 1
        Do
            Dim detailhtml = Await GetAsync(String.Format(DetailUri, now.Year, now.Month.ToString().PadLeft(2, "0"c), now.Day, i, offset, OrderQueryMap(order), If(descending, "DESC", String.Empty)))
            Dim doc As New HtmlDocument()
            doc.LoadHtml(detailhtml)
            Dim oldsize = list.Count
            list.AddRange(
                From tr In doc.DocumentNode.Element("html").Element("body").Element("table").Element("tr").Elements("td").Last().Elements("table").Last().Elements("tr").Skip(1)
                Let tds = (From td In tr.Elements("td").Skip(1)
                           Select td.FirstChild?.InnerText).ToArray()
                Select New NetDetail(
                        Date.ParseExact(tds(1), DateTimeFormat, CultureInfo.InvariantCulture),
                        Date.ParseExact(tds(2), DateTimeFormat, CultureInfo.InvariantCulture),
                        ByteSize.Parse(tds(4))))
            If list.Count - oldsize < offset Then
                Exit Do
            End If
            i += 1
        Loop
        Return list
    End Function
End Class
