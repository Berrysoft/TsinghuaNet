Imports System.ComponentModel
Imports Eto.Forms
Imports Eto.Serialization.Xaml
Imports TsinghuaNet.Helper

Public Class MainForm
    Inherits Form

    Private WithEvents Model As MainViewModel

    Public Sub New()
        XamlReader.Load(Me)
        Model = New MainViewModel()
        DataContext = Model
        FindChild(Of Label)("OnlineUserFluxLabel").
            TextBinding.BindDataContext(
                Binding.Property(Function(m As MainViewModel) m.OnlineUser.Flux).
                    Convert(AddressOf StringHelper.GetFluxString))
        FindChild(Of Label)("OnlineTimeLabel").
            TextBinding.BindDataContext(
                Binding.Property(Function(m As MainViewModel) m.OnlineUser.OnlineTime).
                    Convert(Function(t As TimeSpan) t.ToString()))
        FindChild(Of Label)("OnlineUserBalanceLabel").
            TextBinding.BindDataContext(
                Binding.Property(Function(m As MainViewModel) m.OnlineUser.Balance).
                    Convert(AddressOf StringHelper.GetCurrencyString))
    End Sub

    Private Sub NetStateList_SelectedIndexChanged(sender As Object, e As EventArgs)
        ' 规避Bug
        Model.Credential.State = FindChild(Of RadioButtonList)("NetStateList").SelectedIndex + 1
    End Sub

    Private Sub Model_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles Model.PropertyChanged
        Select Case e.PropertyName
            Case "SuggestState"
                FindChild(Of RadioButtonList)("NetStateList").SelectedIndex = Model.SuggestState - 1
        End Select
    End Sub
End Class
