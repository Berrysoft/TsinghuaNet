Imports Windows.Security.Credentials

Public Module CredentialHelper
    Private Const CredentialResource As String = "TsinghuaNetUWP"
    Private vault As New PasswordVault

    Public Function GetCredential(username As String) As String
        For Each cre In From c In vault.RetrieveAll()
                        Where c.Resource = CredentialResource AndAlso c.UserName = username
            cre.RetrievePassword()
            Return cre.Password
        Next
        Return Nothing
    End Function

    Public Sub SaveCredential(username As String, password As String)
        vault.Add(New PasswordCredential(CredentialResource, username, password))
    End Sub

    Public Sub RemoveCredential(username As String)
        For Each cre In From c In vault.RetrieveAll()
                        Where c.Resource = CredentialResource AndAlso c.UserName = username
            vault.Remove(cre)
        Next
    End Sub
End Module
