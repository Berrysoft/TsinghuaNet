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
        DetailsView.ItemsSource = details
    End Sub

    Private Sub DetailsView_Sorting(sender As Object, e As DataGridColumnEventArgs)
        If details IsNot Nothing Then
            Dim dir As DataGridSortDirection?
            Dim oridir = e.Column.SortDirection
            For Each c In DetailsView.Columns
                c.SortDirection = Nothing
            Next
            Select Case oridir
                Case DataGridSortDirection.Ascending
                    dir = DataGridSortDirection.Descending
                Case DataGridSortDirection.Descending
                    dir = Nothing
                Case Else
                    dir = DataGridSortDirection.Ascending
            End Select
            e.Column.SortDirection = dir
            If dir IsNot Nothing Then
                Dim ascending As Boolean = dir.Value = DataGridSortDirection.Ascending
                Select Case e.Column.Tag
                    Case "LoginTime"
                        DetailsView.ItemsSource = details.OrderBy(Function(d) d.LoginTime, ascending)
                    Case "LogoutTime"
                        DetailsView.ItemsSource = details.OrderBy(Function(d) d.LogoutTime, ascending)
                    Case "Flux"
                        DetailsView.ItemsSource = details.OrderBy(Function(d) d.Flux, ascending)
                End Select
            Else
                DetailsView.ItemsSource = details
            End If
        End If
    End Sub
End Class

Module OrderByHelper
    <Extension>
    Public Function OrderBy(Of TSource, TKey)(source As IEnumerable(Of TSource), selector As Func(Of TSource, TKey), ascending As Boolean) As IOrderedEnumerable(Of TSource)
        If ascending Then
            Return source.OrderBy(selector)
        Else
            Return source.OrderByDescending(selector)
        End If
    End Function
End Module
