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

    Private Const AscendingChar As Char = "▲"c
    Private Const DescendingChar As Char = "▼"c

    Private lastOrder As NetDetailOrder?

    Private Sub DetailsView_ColumnHeaderClick(sender As Object, e As GridColumnEventArgs)
        If Model.DetailsSource.Count > 0 Then
            Dim DetailsView As GridView = sender
            Dim index = DetailsView.Columns.IndexOf(e.Column)
            If lastOrder Is Nothing Then
                lastOrder = index * 2
                e.Column.HeaderText &= AscendingChar
            Else
                If lastOrder.Value \ 2 = index Then
                    Dim t = e.Column.HeaderText
                    If lastOrder.Value Mod 2 = 0 Then
                        lastOrder += 1
                        e.Column.HeaderText = t.Substring(0, t.Length - 1) & DescendingChar
                    Else
                        lastOrder = Nothing
                        e.Column.HeaderText = t.Substring(0, t.Length - 1)
                    End If
                Else
                    Dim oldc = DetailsView.Columns(lastOrder.Value \ 2)
                    Dim oldt = oldc.HeaderText
                    oldc.HeaderText = oldt.Substring(0, oldt.Length - 1)
                    lastOrder = index * 2
                    e.Column.HeaderText &= AscendingChar
                End If
            End If
            Model.SortSource(lastOrder)
        End If
    End Sub
End Class
