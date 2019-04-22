Imports MvvmHelpers
Imports TsinghuaNetUWP.Helper

Public NotInheritable Class EditSuggestionDialog
    Inherits ContentDialog

    Public ReadOnly Property WlanList As New ObservableRangeCollection(Of NetStateSsidBox)

    Private Sub AddSelection(sender As Object, e As RoutedEventArgs)
        AddFlyout.ShowAt(e.OriginalSource)
    End Sub

    Private Sub AddButtonClick()
        Dim ssid As String = AddFlyoutText.Text
        If Not String.IsNullOrEmpty(ssid) Then
            AddFlyout.Hide()
            AddFlyoutText.Text = String.Empty
            If Not Aggregate pair In WlanList
                   Into AnyMatch = Any(pair?.Ssid = ssid) Then
                Dim item As New NetStateSsidBox(ssid, NetState.Unknown)
                WlanList.Add(item)
            End If
        End If
    End Sub

    Private Sub DeleteSelection()
        Dim index = WlanListView.SelectedIndex
        If index >= 0 Then
            WlanList.RemoveAt(index)
        End If
    End Sub

    Private Sub RestoreSelection()
        RefreshWlanList(SettingsHelper.DefWlanStates())
    End Sub

    Friend Sub RefreshWlanList(list As Dictionary(Of String, NetState))
        WlanList.ReplaceRange(
            From pair In list
            Select New NetStateSsidBox(pair.Key, pair.Value))
    End Sub
End Class
