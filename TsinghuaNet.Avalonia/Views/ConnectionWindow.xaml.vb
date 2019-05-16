Imports Avalonia
Imports Avalonia.Controls
Imports Avalonia.Markup.Xaml

Public Class ConnectionWindow
    Inherits Window

    Public Sub New()
        InitializeComponent()
#If DEBUG Then
        AttachDevTools()
#End If
        Program.Selector.EnableThemes(Me)
        AddHandler CType(DataContext, ConnectionViewModel).Drop, AddressOf Model_Drop
    End Sub

    Private Sub InitializeComponent()
        AvaloniaXamlLoader.Load(Me)
    End Sub

    Private Async Sub Model_Drop()
        Dim dg As DataGrid = FindControl(Of DataGrid)("ConnectionView")
        Await CType(DataContext, ConnectionViewModel).DropAsync(dg.SelectedItems.Cast(Of NetUser)().Select(Function(u) u.Address))
    End Sub
End Class
