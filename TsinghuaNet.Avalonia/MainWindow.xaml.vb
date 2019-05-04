Imports Avalonia
Imports Avalonia.Controls
Imports Avalonia.Markup.Xaml

Public Class MainWindow
    Inherits Window

    Public Sub New()
        InitializeComponent()
#If DEBUG Then
        AttachDevTools()
#End If
    End Sub

    Private Sub InitializeComponent()
        AvaloniaXamlLoader.Load(Me)
    End Sub
End Class
