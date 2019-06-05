Imports System.Windows.Input

Public MustInherit Class DetailViewModel
    Inherits NetObservableBase

    Protected MustOverride Property InitialDetails As IEnumerable(Of NetDetail)
    Protected MustOverride Sub SetSortedDetails(source As IEnumerable(Of NetDetail))

    Public Sub New()
        InitializeDetails()
    End Sub

    Private Async Sub InitializeDetails()
        Try
            Dim helper = Credential.GetUseregHelper()
            Await helper.LoginAsync()
            InitialDetails = Await helper.GetDetailsAsync(NetDetailOrder.LogoutTime, False)
        Catch ex As Exception
            Debug.WriteLine(ex)
        End Try
    End Sub

    Public ReadOnly Property RefreshCommand As ICommand = New Command(Me, AddressOf InitializeDetails)

    Public Sub SortSource(order As NetDetailOrder?, descending As Boolean)
        If order Is Nothing Then
            SetSortedDetails(InitialDetails)
        Else
            Select Case order.Value
                Case NetDetailOrder.LoginTime
                    SetSortedDetails(InitialDetails.OrderBy(Function(d) d.LoginTime, descending))
                Case NetDetailOrder.LogoutTime
                    SetSortedDetails(InitialDetails.OrderBy(Function(d) d.LogoutTime, descending))
                Case NetDetailOrder.Flux
                    SetSortedDetails(InitialDetails.OrderBy(Function(d) d.Flux, descending))
            End Select
        End If
    End Sub
End Class
