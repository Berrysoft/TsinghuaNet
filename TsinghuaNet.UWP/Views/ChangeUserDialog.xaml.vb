Imports TsinghuaNet.UWP.Helper

Public NotInheritable Class ChangeUserDialog
    Inherits ContentDialog

    Public Sub New(username As String)
        InitializeComponent()
        UnBox.Text = username
        UsernameChanged()
    End Sub

    Private Sub UsernameChanged()
        Dim un As String = UnBox.Text
        Dim pw As String = If(CredentialHelper.GetCredential(un), String.Empty)
        IsPrimaryButtonEnabled = Not (String.IsNullOrEmpty(un) OrElse String.IsNullOrEmpty(pw))
        PwBox.Password = pw
        SaveBox.IsChecked = Not String.IsNullOrEmpty(pw)
    End Sub

    Private Sub PasswordChanged()
        IsPrimaryButtonEnabled = Not (String.IsNullOrEmpty(UnBox.Text) OrElse String.IsNullOrEmpty(PwBox.Password))
    End Sub
End Class
