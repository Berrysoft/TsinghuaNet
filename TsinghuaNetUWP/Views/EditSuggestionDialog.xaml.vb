Imports TsinghuaNetUWP.Helper

Public NotInheritable Class EditSuggestionDialog
    Inherits ContentDialog

    Public ReadOnly Property WlanList As New ObservableCollection(Of NetStateSsidBox)

    Private Sub AddSelection(sender As Object, e As RoutedEventArgs)
        AddFlyout.ShowAt(e.OriginalSource)
    End Sub

    Private Sub AddButtonClick()
        Dim ssid As String = AddFlyoutText.Text
        If Not String.IsNullOrEmpty(ssid) Then
            AddFlyout.Hide()
            AddFlyoutText.Text = String.Empty
            If Not Aggregate pair In WlanList
                   Into AnyMatch = Any(pair IsNot Nothing AndAlso pair.Ssid = ssid) Then
                Dim item As New NetStateSsidBox()
                item.Ssid = ssid
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

    Private Sub HelpSelection(sender As Object, e As RoutedEventArgs)
        HelpFlyout.ShowAt(e.OriginalSource)
    End Sub

    Private Sub RestoreSelection()
        RefreshWlanList(SettingsHelper.DefWlanStates())
    End Sub

    Friend Sub RefreshWlanList(list As IDictionary(Of String, NetState))
        WlanList.Clear()
        For Each pair In list
            Dim item As New NetStateSsidBox
            item.Ssid = pair.Key
            item.Value = pair.Value
            WlanList.Add(item)
        Next
    End Sub
End Class
