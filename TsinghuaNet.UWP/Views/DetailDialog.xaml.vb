Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports TsinghuaNet.Helper

Public NotInheritable Class DetailDialog
    Inherits ContentDialog

    Public Sub New(helper As UseregHelper)
        InitializeComponent()
    End Sub

    Private Sub DetailsView_Sorting(sender As Object, e As DataGridColumnEventArgs)
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
                    Model.SortSource(NetDetailOrder.LoginTime, Not ascending)
                Case "LogoutTime"
                    Model.SortSource(NetDetailOrder.LogoutTime, Not ascending)
                Case "Flux"
                    Model.SortSource(NetDetailOrder.Flux, Not ascending)
            End Select
        Else
            Model.SortSource(Nothing)
        End If
    End Sub
End Class
