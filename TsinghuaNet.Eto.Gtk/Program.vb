Imports System.Text
Imports Eto

Module Program
    Sub Main()
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
        Using app As New App(Platforms.Gtk), form As New MainForm
            app.Run(form)
        End Using
    End Sub
End Module
