Imports Avalonia
Imports Avalonia.Controls
Imports Avalonia.Markup.Xaml

Public Class ConnectionWindow
    Inherits Window
    Implements IDialogWithModel(Of MainViewModel)

    Public Sub New()
        InitializeComponent()
#If DEBUG Then
        AttachDevTools()
#End If
        Program.Selector.EnableThemes(Me)
        DataContext = Me
    End Sub

    Private Sub InitializeComponent()
        AvaloniaXamlLoader.Load(Me)
    End Sub

    Private helper As UseregHelper

    Friend ReadOnly Property ConnectionView As DataGrid
        Get
            Return FindControl(Of DataGrid)("ConnectionView")
        End Get
    End Property


    Public Overloads Async Function ShowDialog(owner As Window, model As MainViewModel) As Task Implements IDialogWithModel(Of MainViewModel).ShowDialog
        Dim showtask = ShowDialog(owner)
        helper = model.GetUseregHelper()
        Await helper.LoginAsync()
        Await RefreshUsersAsync()
        Await showtask
    End Function

    Private Async Function RefreshUsersAsync() As Task
        ConnectionView.Items = Await helper.GetUsersAsync()
    End Function

    Private Async Sub DropSelection()
        For Each user As NetUser In ConnectionView.SelectedItems
            Await helper.LogoutAsync(user.Address)
        Next
        Await RefreshUsersAsync()
    End Sub
End Class
