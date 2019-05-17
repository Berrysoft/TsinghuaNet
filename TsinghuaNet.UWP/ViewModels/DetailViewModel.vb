Public Class DetailViewModel
    Inherits TsinghuaNet.Helper.DetailViewModel

    Private details As List(Of NetDetail)

    Private _DetailsSource As IEnumerable(Of NetDetail)
    Public Property DetailsSource As IEnumerable(Of NetDetail)
        Get
            Return _DetailsSource
        End Get
        Set(value As IEnumerable(Of NetDetail))
            SetProperty(_DetailsSource, value)
        End Set
    End Property

    Protected Overrides Property InitialDetails As IEnumerable(Of NetDetail)
        Get
            Return details
        End Get
        Set(value As IEnumerable(Of NetDetail))
            details = value
            DetailsSource = details
        End Set
    End Property

    Protected Overrides Sub SetSortedDetails(source As IEnumerable(Of NetDetail))
        DetailsSource = source
    End Sub
End Class
