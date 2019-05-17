Imports Eto.Forms
Imports Eto.Serialization.Xaml

Public Class MainForm
    Inherits Form

    Private Model As MainViewModel

    Public Sub New()
        XamlReader.Load(Me)
        Model = New MainViewModel()
        DataContext = Model
    End Sub

    Private Sub NetStateList_SelectedIndexChanged(sender As Object, e As EventArgs)
        ' 规避Bug
        Model.Credential.State = FindChild(Of RadioButtonList)("NetStateList").SelectedIndex + 1
    End Sub

End Class
