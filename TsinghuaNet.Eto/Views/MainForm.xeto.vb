Imports Eto.Forms
Imports Eto.Serialization.Xaml
Imports TsinghuaNet.Helper

Public Class MainForm
    Inherits Form

    Private Model As MainViewModel

    Public Sub New()
        XamlReader.Load(Me)
        Model = New MainViewModel()
        DataContext = Model
        Dim NetStateList = FindChild(Of EnumRadioButtonList(Of NetState))("NetStateList")
        AddHandler NetStateList.AddValue, AddressOf NetStateList_AddValue
        NetStateList.SelectedValueBinding.BindDataContext(Binding.Property(Function(m As MainViewModel) m.Credential.State))
        FindChild(Of Label)("OnlineUserBalanceLabel").
            TextBinding.BindDataContext(
                Binding.Property(Function(m As MainViewModel) m.OnlineUser.Balance).
                    Convert(AddressOf StringHelper.GetCurrencyString))
    End Sub

    Private Sub NetStateList_AddValue(sender As Object, e As AddValueEventArgs(Of NetState))
        If e.Value = NetState.Unknown Then
            e.ShouldAdd = False
        End If
    End Sub

    Private Sub ShowConnection(sender As Object, e As EventArgs)
        Using dialog As New ConnectionDialog
            dialog.ShowModal(Me)
        End Using
    End Sub

    Private Sub ShowDetails(sender As Object, e As EventArgs)
        Using dialog As New DetailsDialog
            dialog.ShowModal(Me)
        End Using
    End Sub

    Private Sub ShowAbout(sender As Object, e As EventArgs)
        Using dialog As New SettingsDialog(Model, 1)
            dialog.ShowModal(Me)
        End Using
    End Sub

    Private Sub ShowSettings(sender As Object, e As EventArgs)
        Using dialog As New SettingsDialog(Model)
            dialog.ShowModal(Me)
        End Using
    End Sub

    Private Sub MainForm_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        Model.SaveSettings()
    End Sub
End Class
