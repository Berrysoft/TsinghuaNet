Imports System.Net.Http
Imports System.Text

Public Interface ILog
    Inherits IDisposable

    Function LoginAsync() As Task(Of LogResponse)
    Function LogoutAsync() As Task(Of LogResponse)
End Interface

Public Interface IConnect
    Inherits ILog

    Function GetFluxAsync() As Task(Of FluxUser)
End Interface

Public MustInherit Class NetHelperBase
    Implements IDisposable

    Private client As HttpClient

    Public ReadOnly Property Username As String

    Public ReadOnly Property Password As String

    Public Sub New(username As String, password As String)
        Me.New(username, password, Nothing)
    End Sub

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

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
    End Sub
#End Region
End Class
