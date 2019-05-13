Imports System.Text

Class Application
    Private Sub Application_Startup(sender As Object, e As StartupEventArgs)
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
    End Sub
End Class
