Imports Eto.Forms
Imports Eto.Serialization.Xaml

Public Class MainForm
    Inherits Form

    Public Sub New()
        XamlReader.Load(Me)
    End Sub

End Class
