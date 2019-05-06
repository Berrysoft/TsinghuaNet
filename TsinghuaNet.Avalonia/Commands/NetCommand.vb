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
            Await ExecuteAsync(helper)
        Finally
            executing = False
            OnCanExecuteChanged(EventArgs.Empty)
        End Try
    End Sub

    Protected MustOverride Function ExecuteAsync(helper As IConnect) As Task

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

    Protected Overrides Async Function ExecuteAsync(helper As IConnect) As Task
        If helper IsNot Nothing Then
            Await helper.LoginAsync()
        End If
        Await model.RefreshAsync(helper)
    End Function
End Class

Class LogoutCommand
    Inherits NetCommand

    Public Sub New(model As MainViewModel)
        MyBase.New(model)
    End Sub

    Protected Overrides Async Function ExecuteAsync(helper As IConnect) As Task
        If helper IsNot Nothing Then
            Await helper.LogoutAsync()
        End If
        Await model.RefreshAsync(helper)
    End Function
End Class

Class RefreshCommand
    Inherits NetCommand

    Public Sub New(model As MainViewModel)
        MyBase.New(model)
    End Sub

    Protected Overrides Async Function ExecuteAsync(helper As IConnect) As Task
        Await model.RefreshAsync(helper)
    End Function
End Class
