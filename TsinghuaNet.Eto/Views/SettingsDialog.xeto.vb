Imports Eto.Forms
Imports Eto.Serialization.Xaml

Public Class SettingsDialog
    Inherits Dialog

    Public Sub New()
        XamlReader.Load(Me)
    End Sub
End Class
