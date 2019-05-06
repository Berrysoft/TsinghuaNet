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
    End Sub

    Private Sub InitializeComponent()
        AvaloniaXamlLoader.Load(Me)
    End Sub

    Public Overloads Async Function ShowDialog(owner As Window, model As MainViewModel) As Task Implements IDialogWithModel(Of MainViewModel).ShowDialog
        Dim showtask = ShowDialog(owner)
        Dim helper = model.GetUseregHelper()
        Await helper.LoginAsync()
        Dim grid = FindControl(Of DataGrid)("ConnectionView")
        grid.Items = Await helper.GetUsersAsync()
        Await showtask
    End Function
End Class
