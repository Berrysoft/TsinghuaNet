Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports MvvmHelpers
Imports TsinghuaNet.Helper

Public NotInheritable Class DetailDialog
    Inherits ContentDialog

    Public ReadOnly Property Details As New ObservableRangeCollection(Of KeyValuePair(Of Integer, Double))

    Public Sub New()
        InitializeComponent()
        XAxis.Interval = 1
        YAxis.Minimum = 0
    End Sub

    Public Shared ReadOnly ChartBrushOffsetProperty As DependencyProperty = DependencyProperty.Register(NameOf(ChartBrushOffset), GetType(Double), GetType(DetailDialog), New PropertyMetadata(1.0))
    Public Property ChartBrushOffset As Double
        Get
            Return GetValue(ChartBrushOffsetProperty)
        End Get
        Set(value As Double)
            SetValue(ChartBrushOffsetProperty, value)
        End Set
    End Property

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
            Model.SortSource(Nothing, False)
        End If
    End Sub

    Private Sub Model_DetailsInitialized(sender As Object, e As IEnumerable(Of NetDetail))
        Details.ReplaceRange(e.GetDailyDetails())
    End Sub
End Class

Module NetDetailExtensions
    <Extension>
    Public Iterator Function GetDailyDetails(ds As IEnumerable(Of NetDetail)) As IEnumerable(Of KeyValuePair(Of Integer, Double))
        Dim totalf As Double = 0
        Dim now As Date = Date.Now
        For Each b In From d In ds
                      Group By d.LogoutTime.Day Into Flux = Sum(d.Flux)
            totalf += b.Flux.GigaBytes
            Yield New KeyValuePair(Of Integer, Double)(b.Day, totalf)
        Next
    End Function
End Module
