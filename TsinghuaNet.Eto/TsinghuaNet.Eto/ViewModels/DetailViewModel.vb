Imports MvvmHelpers

Public Class DetailViewModel
    Inherits Models.DetailViewModel

    Public Sub New()
        MyBase.New()
    End Sub

    Private details As List(Of NetDetail)

    Public ReadOnly Property DetailsSource As New ObservableRangeCollection(Of NetDetail)

    Protected Overrides Property InitialDetails As IEnumerable(Of NetDetail)
        Get
            Return details
        End Get
        Set(value As IEnumerable(Of NetDetail))
            details = value
            DetailsSource.ReplaceRange(details)
        End Set
    End Property

    Protected Overrides Sub SetSortedDetails(source As IEnumerable(Of NetDetail))
        DetailsSource.ReplaceRange(source)
    End Sub
End Class
