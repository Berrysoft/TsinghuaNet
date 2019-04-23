Imports System.Net
Imports Berrysoft.Tsinghua.Net

Module NetUserHelper
    Public Function Equals(user As NetUser, other As NetUser) As Boolean
        Return user.Address.Equals(other.Address) AndAlso user.LoginTime = other.LoginTime AndAlso user.Client = other.Client
    End Function
End Module

Public NotInheritable Class DropCommand
    Implements ICommand

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged
    Public Event DropUser As EventHandler(Of IPAddress)

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        RaiseEvent DropUser(Me, parameter)
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
    End Function
End Class
