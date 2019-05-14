Imports System.Windows.Input

Public Class NetModel
    Inherits NetObservableBase

    Private _Status As INetStatus
    Public Property Status As INetStatus
        Get
            Return _Status
        End Get
        Set(value As INetStatus)
            SetProperty(_Status, value, onChanged:=AddressOf RefreshStatus)
        End Set
    End Property

    Public ReadOnly Property RefreshStatusCommand As ICommand = New Command(AddressOf RefreshStatus)
    Private Async Sub RefreshStatus()
        Try
            IsBusy = True
            Await Status.RefreshAsync()
            Credential.State = Await Status.SuggestAsync()
        Finally
            IsBusy = False
        End Try
    End Sub

    Private _OnlineUser As FluxUser
    Public Property OnlineUser As FluxUser
        Get
            Return _OnlineUser
        End Get
        Set(value As FluxUser)
            SetProperty(_OnlineUser, value)
        End Set
    End Property

    Public ReadOnly Property LoginCommand As ICommand = New NetCommand(Me, AddressOf LoginAsync)
    Private Async Function LoginAsync(helper As IConnect) As Task(Of LogResponse)
        If helper IsNot Nothing Then
            Dim res = Await helper.LoginAsync()
            If Not res.Succeed Then
                Return res
            End If
        End If
        Await RefreshAsync(helper)
        Return New LogResponse(True, "登录成功")
    End Function

    Public ReadOnly Property LogoutCommand As ICommand = New NetCommand(Me, AddressOf LogoutAsync)
    Private Async Function LogoutAsync(helper As IConnect) As Task(Of LogResponse)
        If helper IsNot Nothing Then
            Dim res = Await helper.LogoutAsync()
            If Not res.Succeed Then
                Return res
            End If
        End If
        Await RefreshAsync(helper)
        Return New LogResponse(True, "注销成功")
    End Function

    Public ReadOnly Property RefreshCommand As ICommand = New NetCommand(Me, AddressOf RefreshAsync)
    Private Async Function RefreshAsync(helper As IConnect) As Task(Of LogResponse)
        Dim user As FluxUser = Nothing
        If helper IsNot Nothing Then
            user = Await helper.GetFluxAsync()
        End If
        OnlineUser = user
        Return New LogResponse(True, String.Empty)
    End Function
End Class
