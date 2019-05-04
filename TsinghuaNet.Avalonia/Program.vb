Imports Avalonia
Imports Avalonia.Logging.Serilog

Module Program
    Sub Main(args As String())
        BuildAvaloniaApp().Start(AddressOf AppMain, args)
    End Sub

    Function BuildAvaloniaApp() As AppBuilder
        Return AppBuilder.Configure(Of App)().UsePlatformDetect().LogToDebug()
    End Function

    Private Sub AppMain(app As Application, args As String())
        app.Run(New MainWindow())
    End Sub
End Module
