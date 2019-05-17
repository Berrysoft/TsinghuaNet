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
    End Sub
End Class
