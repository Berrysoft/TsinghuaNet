Imports MvvmHelpers
Imports TsinghuaNet

Public Class DetailViewModel
    Inherits Helper.DetailViewModel

    Public Sub New()
        MyBase.New()
    End Sub

    Public ReadOnly Property DetailsSource As New ObservableRangeCollection(Of NetDetail)

    Protected Overrides Property InitialDetails As IEnumerable(Of NetDetail)
        Get
            Return DetailsSource
        End Get
        Set(value As IEnumerable(Of NetDetail))
            DetailsSource.ReplaceRange(value)
        End Set
    End Property

    Protected Overrides Sub SetSortedDetails(source As IEnumerable(Of NetDetail))
        DetailsSource.ReplaceRange(source)
    End Sub
End Class
