Imports System.Net

Public NotInheritable Class DropCommand
    Implements ICommand

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged
    Public Event DropUser As EventHandler(Of IPAddress)

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        RaiseEvent DropUser(Me, parameter)
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return parameter IsNot Nothing
    End Function
End Class
