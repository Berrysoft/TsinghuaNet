Imports System.Windows.Input
Imports Avalonia
Imports Avalonia.Controls

Public Class ShowDialogCommand(Of T As {Window, New})
    Implements ICommand

    Private ReadOnly model As MainViewModel
    Public Sub New(model As MainViewModel)
        Me.model = model
    End Sub

    Public Async Sub Execute(parameter As Object) Implements ICommand.Execute
        Dim win As New T
        Await win.ShowDialog(Application.Current.MainWindow)
    End Sub

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
    End Function
End Class
