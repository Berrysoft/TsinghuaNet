Imports Eto.Forms
Imports Eto.Serialization.Xaml

Public Class SettingsDialog
    Inherits Dialog

    Private Model As SettingsViewModel

    Public Sub New(Optional page As Integer = 0)
        XamlReader.Load(Me)
        Model = New SettingsViewModel()
        DataContext = Model
        FindChild(Of TabControl)("SettingsTab").SelectedIndex = page
        FindChild(Of GridView)("PackageView").DataStore = Model.Packages
    End Sub
End Class
