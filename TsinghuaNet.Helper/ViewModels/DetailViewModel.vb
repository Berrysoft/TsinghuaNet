Imports System.Windows.Input

Public Class DetailViewModel
    Inherits NetObservableBase

    Private details As List(Of NetDetail)

    Public Sub New()
        InitializeDetails()
    End Sub

    Private Async Sub InitializeDetails()
        Try
            Dim helper = Credential.GetUseregHelper()
            Await helper.LoginAsync()
            details = New List(Of NetDetail)(Await helper.GetDetailsAsync())
            DetailsSource = details
        Catch ex As Exception
            Debug.WriteLine(ex)
        End Try
    End Sub

    Private _DetailsSource As IEnumerable(Of NetDetail)
    Public Property DetailsSource As IEnumerable(Of NetDetail)
        Get
            Return _DetailsSource
        End Get
        Set(value As IEnumerable(Of NetDetail))
            SetProperty(_DetailsSource, value)
        End Set
    End Property

    Public ReadOnly Property RefreshCommand As ICommand = New Command(Me, AddressOf InitializeDetails)

    Public Sub SortSource(order As NetDetailOrder?)
        SortSource(order, order Is Nothing OrElse order.Value Mod 2 <> 0)
    End Sub

    Public Sub SortSource(order As NetDetailOrder?, descending As Boolean)
        If order Is Nothing Then
            DetailsSource = details
        Else
            Select Case order.Value
                Case NetDetailOrder.LoginTime, NetDetailOrder.LoginTimeDescending
                    DetailsSource = details.OrderBy(Function(d) d.LoginTime, descending)
                Case NetDetailOrder.LogoutTime, NetDetailOrder.LogoutTimeDescending
                    DetailsSource = details.OrderBy(Function(d) d.LogoutTime, descending)
                Case NetDetailOrder.Flux, NetDetailOrder.FluxDescending
                    DetailsSource = details.OrderBy(Function(d) d.Flux, descending)
            End Select
        End If
    End Sub
End Class
