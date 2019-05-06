Imports System.Windows.Input

Class DropCommand
    Implements ICommand

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        Throw New NotImplementedException()
    End Sub

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
    End Function
End Class
