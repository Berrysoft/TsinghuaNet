Imports System.Windows.Input

Public Class Command
    Implements ICommand

    Private action As Action(Of Object)
    Public Sub New(action As Action)
        Me.New(Sub(o) action())
    End Sub
    Public Sub New(action As Action(Of Object))
        Me.action = action
    End Sub

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        action?.Invoke(parameter)
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
    End Function
End Class
