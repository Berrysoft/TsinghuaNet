Imports Eto.Forms
Imports Eto.Serialization.Xaml
Imports TsinghuaNet.Helper

Public Class DetailsDialog
    Inherits Dialog

    Private Model As DetailViewModel

    Public Sub New()
        XamlReader.Load(Me)
        Model = New DetailViewModel()
        DataContext = Model
        Dim DetailsView = FindChild(Of GridView)("DetailsView")
        DetailsView.DataStore = Model.DetailsSource
        CType(DetailsView.Columns(2).DataCell, TextBoxCell).Binding =
            Binding.Property(Function(d As NetDetail) d.Flux).
                Convert(AddressOf StringHelper.GetFluxString)
    End Sub
End Class
