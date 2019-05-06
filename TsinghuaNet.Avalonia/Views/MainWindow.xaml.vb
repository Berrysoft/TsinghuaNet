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
        Program.Selector.EnableThemes(Me)
    End Sub

    Private Sub InitializeComponent()
        AvaloniaXamlLoader.Load(Me)
    End Sub

    Private Async Sub MainWindow_Opened(sender As Object, e As EventArgs) Handles Me.Opened
        Dim model As MainViewModel = DataContext
        Await model.LoadSettingsAsync()
    End Sub

    Private Sub MainWindow_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        Dim model As MainViewModel = DataContext
        model.SaveSettings()
    End Sub
End Class
