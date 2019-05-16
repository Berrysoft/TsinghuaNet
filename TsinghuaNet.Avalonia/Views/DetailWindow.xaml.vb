Imports Avalonia
Imports Avalonia.Controls
Imports Avalonia.Markup.Xaml

Public Class DetailWindow
    Inherits Window

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
End Class
