Imports Eto

Module Program
    Sub Main()
        Using app As New App(Platforms.Wpf), form As New MainForm
            app.Run(form)
        End Using
    End Sub
End Module
