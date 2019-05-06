Imports System.Windows.Input
Imports TsinghuaNet.Helper

Public Class NetStateChangeCommand
    Implements ICommand

    Private ReadOnly model As MainViewModel
    Public Sub New(model As MainViewModel)
        Me.model = model
    End Sub

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        model.State = [Enum].Parse(Of NetState)(parameter)
    End Sub

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged
    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
    End Function
End Class
