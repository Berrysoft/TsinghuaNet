Imports Eto.Forms
Imports Eto.Serialization.Xaml

Public Class SettingsDialog
    Inherits Dialog

    Public Sub New(Optional page As Integer = 0)
        XamlReader.Load(Me)
        FindChild(Of TabControl)("SettingsTab").SelectedIndex = page
    End Sub
End Class
