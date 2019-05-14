Imports System.ComponentModel
Imports System.Windows.Input

Public Class Command
    Implements ICommand

    Private viewModel As NetObservableBase
    Private action As Action(Of Object)
    Public Sub New(viewModel As NetObservableBase, action As Action)
        Me.New(viewModel, Sub(o) action())
    End Sub
    Public Sub New(viewModel As NetObservableBase, action As Action(Of Object))
        Me.viewModel = viewModel
        Me.action = action
        AddHandler viewModel.PropertyChanged, AddressOf OnModelPropertyChanged
    End Sub

    Private Sub OnModelPropertyChanged(sender As Object, e As PropertyChangedEventArgs)
        If e.PropertyName = "IsBusy" Then
            OnCanExecuteChanged()
        End If
    End Sub

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged
    Protected Overridable Sub OnCanExecuteChanged()
        RaiseEvent CanExecuteChanged(Me, EventArgs.Empty)
    End Sub

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        Try
            viewModel.IsBusy = True
            action?.Invoke(parameter)
        Finally
            viewModel.IsBusy = False
        End Try
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return Not viewModel.IsBusy
    End Function
End Class
