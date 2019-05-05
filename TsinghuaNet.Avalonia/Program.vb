Imports Avalonia
Imports Avalonia.Logging.Serilog
Imports Avalonia.ThemeManager

Module Program
    Sub Main(args As String())
        Try
            BuildAvaloniaApp().Start(AddressOf AppMain, args)
        Catch ex As Exception
            Console.WriteLine(ex)
        End Try
    End Sub

    Function BuildAvaloniaApp() As AppBuilder
        Return AppBuilder.Configure(Of App)().UsePlatformDetect().LogToDebug()
    End Function

    Public Selector As IThemeSelector

    Private Sub AppMain(app As Application, args As String())
        Selector = ThemeSelector.Create("Themes")
        Selector.LoadSelectedTheme("TsinghuaNet.Avalonia.theme")
        app.Run(New MainWindow())
        Selector.SaveSelectedTheme("TsinghuaNet.Avalonia.theme")
    End Sub
End Module
