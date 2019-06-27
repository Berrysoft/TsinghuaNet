Imports Eto.Forms
Imports Eto.Serialization.Xaml
Imports TsinghuaNet.Helpers
Imports TsinghuaNet.Models

Public Class ConnectionDialog
    Inherits Dialog

    Private Model As ConnectionViewModel

    Public Sub New()
        XamlReader.Load(Me)
        Model = New ConnectionViewModel()
        DataContext = Model
        FindChild(Of GridView)("ConnectionView").DataStore = Model.NetUsers
        Model.RefreshNetUsers()
    End Sub

    Private Async Sub DropSelection(sender As Object, e As EventArgs)
        Dim view = FindChild(Of GridView)("ConnectionView")
        Await Model.DropAsync(view.SelectedItems.Select(Function(user As NetUser) user.Address))
    End Sub
End Class
