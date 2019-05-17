Imports Eto.Forms
Imports Eto.Serialization.Xaml
Imports TsinghuaNet.Helper

Public Class ConnectionDialog
    Inherits Dialog

    Private Model As ConnectionViewModel

    Public Sub New()
        XamlReader.Load(Me)
        Model = New ConnectionViewModel()
        DataContext = Model
        Model.RefreshNetUsers()
    End Sub

    Private Async Sub DropSelection(sender As Object, e As EventArgs)
        Dim view = FindChild(Of GridView)("ConnectionView")
        Await Model.DropAsync(view.SelectedItems.Select(Function(user As NetUser) user.Address))
    End Sub
End Class
