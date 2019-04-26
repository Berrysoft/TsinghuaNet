Imports Berrysoft.Tsinghua.Net
Imports Microsoft.Toolkit.Uwp.UI.Controls

Public NotInheritable Class DetailDialog
    Inherits ContentDialog

    Private details As List(Of NetDetail)

    Public Sub New(helper As UseregHelper)
        InitializeComponent()
        InitializeDetails(helper)
    End Sub

    Private Async Sub InitializeDetails(helper As UseregHelper)
        details = New List(Of NetDetail)(Await helper.GetDetailsAsync())
        LogoutColumn.SortDirection = DataGridSortDirection.Ascending
        DetailsView.ItemsSource = New ObservableCollection(Of NetDetail)(details)
    End Sub

    Private Sub DetailsView_Sorting(sender As Object, e As DataGridColumnEventArgs)
        If e.Column.SortDirection Is Nothing OrElse e.Column.SortDirection = DataGridSortDirection.Ascending Then
            e.Column.SortDirection = DataGridSortDirection.Descending
            Select Case e.Column.Tag
                Case "LoginTime"

                Case "LogoutTime"
                    DetailsView.ItemsSource = New ObservableCollection(Of NetDetail)(From d In details Order By d.LogoutTime Descending)
                Case "Flux"

            End Select
        Else
            e.Column.SortDirection = DataGridSortDirection.Ascending
            Select Case e.Column.Tag
                Case "LoginTime"

                Case "LogoutTime"
                    DetailsView.ItemsSource = New ObservableCollection(Of NetDetail)(From d In details Order By d.LogoutTime Ascending)
                Case "Flux"

            End Select
        End If
    End Sub
End Class
