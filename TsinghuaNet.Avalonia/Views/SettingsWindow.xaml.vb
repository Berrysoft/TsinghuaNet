Imports System.ComponentModel
Imports Avalonia
Imports Avalonia.Controls
Imports Avalonia.Markup.Xaml
Imports TsinghuaNet.Helper

Public Class SettingsWindow
    Inherits Window
    Implements IDialogWithModel(Of MainViewModel)
    Implements INotifyPropertyChanged

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

    Public Shadows Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private _Model As MainViewModel
    Public Property Model As MainViewModel
        Get
            Return _Model
        End Get
        Set(value As MainViewModel)
            _Model = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Model)))
        End Set
    End Property

    Public ReadOnly Property Packages As New List(Of PackageBox) From
    {
        New PackageBox("Avalonia", "MIT许可证"),
        New PackageBox("Avalonia.Controls.DataGrid", "MIT许可证"),
        New PackageBox("Avalonia.Desktop", "MIT许可证"),
        New PackageBox("Avalonia.ThemeManager", "MIT许可证"),
        New PackageBox("HtmlAgilityPack", "MIT许可证"),
        New PackageBox("Microsoft.DotNet.ILCompiler", "MIT许可证"),
        New PackageBox("Newtonsoft.Json", "MIT许可证"),
        New PackageBox("Refractored.MvvmHelpers", "MIT许可证")
    }

    Public Overloads Function ShowDialog(owner As Window, model As MainViewModel) As Task Implements IDialogWithModel(Of MainViewModel).ShowDialog
        Me.Model = model
        Return ShowDialog(owner)
    End Function
End Class
