Imports Eto.Forms
Imports Eto.Serialization.Xaml
Imports TsinghuaNet.Models

Public Class SettingsDialog
    Inherits Dialog

    Public ReadOnly Property Packages As IReadOnlyList(Of PackageBox) = New List(Of PackageBox) From
    {
        New PackageBox("Eto.Forms", "BSD-3许可证"),
        New PackageBox("Eto.Platform.Gtk", "BSD-3许可证"),
        New PackageBox("Eto.Platform.Mac64", "BSD-3许可证"),
        New PackageBox("Eto.Platform.Wpf", "BSD-3许可证"),
        New PackageBox("Eto.Serialization.Xaml", "BSD-3许可证"),
        New PackageBox("HtmlAgilityPack", "MIT许可证"),
        New PackageBox("Newtonsoft.Json", "MIT许可证"),
        New PackageBox("Refractored.MvvmHelpers", "MIT许可证")
    }

    Public Sub New(model As MainViewModel, Optional page As Integer = 0)
        XamlReader.Load(Me)
        DataContext = model
        FindChild(Of TabControl)("SettingsTab").SelectedIndex = page
        FindChild(Of GridView)("PackageView").DataStore = Packages
    End Sub
End Class
