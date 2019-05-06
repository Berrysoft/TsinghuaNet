Imports Avalonia
Imports Avalonia.Controls
Imports Avalonia.Markup.Xaml

Public Class SettingsWindow
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

    Private ModelProperty As AvaloniaProperty(Of MainViewModel) = AvaloniaProperty.Register(Of SettingsWindow, MainViewModel)(NameOf(Model))
    Public Property Model As MainViewModel
        Get
            Return GetValue(ModelProperty)
        End Get
        Set(value As MainViewModel)
            SetValue(ModelProperty, value)
        End Set
    End Property

    Public Overloads Function ShowDialog(owner As Window, model As MainViewModel) As Task Implements IDialogWithModel(Of MainViewModel).ShowDialog
        Me.Model = model
        Return ShowDialog(owner)
    End Function
End Class
