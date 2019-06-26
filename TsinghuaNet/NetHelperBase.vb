Imports System.Net.Http
Imports System.Text

''' <summary>
''' 提供登录注销方法
''' </summary>
Public Interface ILog
    Inherits IDisposable

    ''' <summary>
    ''' 登录
    ''' </summary>
    ''' <returns>网页回复</returns>
    Function LoginAsync() As Task(Of LogResponse)
    ''' <summary>
    ''' 注销
    ''' </summary>
    ''' <returns>网页回复</returns>
    Function LogoutAsync() As Task(Of LogResponse)
End Interface

''' <summary>
''' 提供查询流量方法
''' </summary>
Public Interface IConnect
    Inherits ILog

    ''' <summary>
    ''' 查询当前在线用户流量
    ''' </summary>
    ''' <returns>当前在线用户</returns>
    Function GetFluxAsync() As Task(Of FluxUser)
End Interface

''' <summary>
''' 提供了HTTP操作的基类型。此类不可继承。
''' </summary>
Public MustInherit Class NetHelperBase
    Implements IDisposable

    Private ReadOnly client As HttpClient

    ''' <summary>
    ''' 用户名
    ''' </summary>
    ''' <returns>用户名</returns>
    Public ReadOnly Property Username As String
    ''' <summary>
    ''' 密码
    ''' </summary>
    ''' <returns>密码</returns>
    Public ReadOnly Property Password As String

    ''' <summary>
    ''' 使用用户名、密码和一个<see cref="HttpClient"/>实例初始化辅助类
    ''' </summary>
    ''' <param name="username">用户名</param>
    ''' <param name="password">密码</param>
    ''' <param name="client">实例</param>
    Public Sub New(username As String, password As String, client As HttpClient)
        Me.Username = username
        Me.Password = password
        Me.client = If(client, New HttpClient())
    End Sub

    Protected Async Function PostAsync(uri As String) As Task(Of String)
        Using message As New HttpRequestMessage(HttpMethod.Post, uri)
            Using response = Await client.SendAsync(message)
                Return Await response.Content.ReadAsStringAsync()
            End Using
        End Using
    End Function

    Protected Async Function PostAsync(uri As String, data As String) As Task(Of String)
        Using content As New StringContent(If(data, String.Empty), Encoding.UTF8, "application/x-www-form-urlencoded")
            Using response = Await client.PostAsync(uri, content)
                Return Await response.Content.ReadAsStringAsync()
            End Using
        End Using
    End Function

    Protected Async Function PostAsync(uri As String, data As Dictionary(Of String, String)) As Task(Of String)
        Using content As New FormUrlEncodedContent(data)
            Using response = Await client.PostAsync(uri, content)
                Return Await response.Content.ReadAsStringAsync()
            End Using
        End Using
    End Function

    Protected Function GetAsync(uri As String) As Task(Of String)
        Return client.GetStringAsync(uri)
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                client.Dispose()
            End If
        End If
        disposedValue = True
    End Sub

    ''' <inheritdoc/>
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
    End Sub
#End Region
End Class
