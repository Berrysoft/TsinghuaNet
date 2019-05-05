Imports Avalonia
Imports Avalonia.Controls
Imports Avalonia.Markup.Xaml

Public Class DetailWindow
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
        Dim details = (Await helper.GetDetailsAsync()).Select(Function(d) New NetDetailBox() With {.LoginTime = d.LoginTime, .LogoutTime = d.LogoutTime, .Flux = d.Flux})
        Dim grid = FindControl(Of DataGrid)("DetailView")
        grid.Items = details
        Await showtask
    End Function
End Class

Class NetDetailBox
    Public Property LoginTime As Date
    Public Property LogoutTime As Date
    Public Property Flux As Long
End Class
