Imports System.Net
Imports System.Windows.Input
Imports MvvmHelpers

Public Class ConnectionViewModel
    Inherits NetObservableBase

    Public ReadOnly Property NetUsers As New ObservableRangeCollection(Of NetUser)

    Public ReadOnly Property RefreshCommand As ICommand = New Command(Me, AddressOf RefreshNetUsers)

    Public Async Sub RefreshNetUsers()
        Await RefreshNetUsersAsync()
    End Sub

    ''' <summary>
    ''' 刷新所有连接情况
    ''' </summary>
    Public Async Function RefreshNetUsersAsync() As Task
        Try
            If Credential.State <> NetState.Unknown Then
                Dim helper = Credential.GetUseregHelper()
                Await helper.LoginAsync()
                Await RefreshNetUsersAsync(helper)
            End If
        Catch ex As Exception
            Debug.WriteLine(ex)
        End Try
    End Function

    ''' <summary>
    ''' 使用给定的帮助类刷新所有连接情况。
    ''' 在调用这个方法前要调用<see cref="UseregHelper.LoginAsync"/>。
    ''' </summary>
    ''' <param name="helper">帮助类实例</param>
    Public Async Function RefreshNetUsersAsync(helper As UseregHelper) As Task
        Dim users = (Await helper.GetUsersAsync()).ToList()
        Dim usersmodel = NetUsers
        Dim i As Integer = 0
        Do While i < usersmodel.Count
            Dim olduser As NetUser = usersmodel(i)
            ' 循环判断旧元素是否存在于新集合中
            For j = 0 To users.Count - 1
                Dim user As NetUser = users(j)
                ' 如果存在则移除新元素
                If olduser = user Then
                    users.RemoveAt(j)
                    i += 1
                    Continue Do
                End If
            Next
            ' 反之移除旧元素
            usersmodel.RemoveAt(i)
        Loop
        ' 最后添加新增元素
        ' 判断大小以防止索引错误
        If users.Count > 0 Then
            usersmodel.AddRange(users)
        End If
    End Function

    Public Function DropAsync(ParamArray ips() As IPAddress) As Task
        Return DropAsync(ips.AsEnumerable())
    End Function

    Public Async Function DropAsync(ips As IEnumerable(Of IPAddress)) As Task
        Try
            If Credential.State <> NetState.Unknown Then
                Dim helper = Credential.GetUseregHelper()
                Await helper.LoginAsync()
                For Each ip In ips
                    Await helper.LogoutAsync(ip)
                Next
                Await RefreshNetUsersAsync(helper)
            End If
        Catch ex As Exception
            Debug.WriteLine(ex)
        End Try
    End Function
End Class
