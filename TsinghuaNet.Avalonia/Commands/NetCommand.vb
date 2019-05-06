Imports System.ComponentModel
Imports System.Windows.Input
Imports TsinghuaNet.Helper

Public MustInherit Class NetCommand
    Implements ICommand

    Protected ReadOnly model As MainViewModel
    Protected executing As Boolean
    Public Sub New(model As MainViewModel)
        Me.model = model
        AddHandler model.PropertyChanged, AddressOf OnModelPropertyChanged
    End Sub

    Private Sub OnModelPropertyChanged(sender As Object, e As PropertyChangedEventArgs)
        If e.PropertyName = "State" Then
            OnCanExecuteChanged(EventArgs.Empty)
        End If
    End Sub

    Public Async Sub Execute(parameter As Object) Implements ICommand.Execute
        executing = True
        OnCanExecuteChanged(EventArgs.Empty)
        Try
            Dim helper = model.GetHelper()
            model.ConnectionSuccess = Await ExecuteAsync(helper)
        Catch ex As Exception
            model.FailMessage = ex.Message
            model.ConnectionSuccess = False
        Finally
            executing = False
            OnCanExecuteChanged(EventArgs.Empty)
        End Try
    End Sub

    Protected MustOverride Function ExecuteAsync(helper As IConnect) As Task(Of Boolean)

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged
    Protected Overridable Sub OnCanExecuteChanged(e As EventArgs)
        RaiseEvent CanExecuteChanged(Me, e)
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return Not executing AndAlso model.State > NetState.Unknown AndAlso model.State <= NetState.Auth6
    End Function
End Class

Class LoginCommand
    Inherits NetCommand

    Public Sub New(model As MainViewModel)
        MyBase.New(model)
    End Sub

    Protected Overrides Async Function ExecuteAsync(helper As IConnect) As Task(Of Boolean)
        If helper IsNot Nothing Then
            Dim res = Await helper.LoginAsync()
            If Not res.Succeed Then
                model.FailMessage = res.Message
                Return False
            End If
        End If
        Await model.RefreshAsync(helper)
        Return True
    End Function
End Class

Class LogoutCommand
    Inherits NetCommand

    Public Sub New(model As MainViewModel)
        MyBase.New(model)
    End Sub

    Protected Overrides Async Function ExecuteAsync(helper As IConnect) As Task(Of Boolean)
        If helper IsNot Nothing Then
            Dim res = Await helper.LogoutAsync()
            If Not res.Succeed Then
                model.FailMessage = res.Message
                Return False
            End If
        End If
        Await model.RefreshAsync(helper)
        Return True
    End Function
End Class

Class RefreshCommand
    Inherits NetCommand

    Public Sub New(model As MainViewModel)
        MyBase.New(model)
    End Sub

    Protected Overrides Async Function ExecuteAsync(helper As IConnect) As Task(Of Boolean)
        Await model.RefreshAsync(helper)
        Return True
    End Function
End Class
