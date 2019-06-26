Imports System.Globalization
Imports System.Text
Imports System.Threading
Imports Eto

Module Program
    Sub Main()
#If NETCOREAPP3_0 Then
        ' ¹æ±Ü.NET Core 3.0.0-preview6µÄbug
        Dim culture = CultureInfo.CreateSpecificCulture("en")
        Thread.CurrentThread.CurrentUICulture = culture
        Thread.CurrentThread.CurrentCulture = culture
        CultureInfo.DefaultThreadCurrentCulture = culture
        CultureInfo.DefaultThreadCurrentUICulture = culture

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
#End If
        Using app As New App(Platforms.Wpf), form As New MainForm
            app.Run(form)
        End Using
    End Sub
End Module
