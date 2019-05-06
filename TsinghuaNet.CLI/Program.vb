Imports System.Text
Imports CommandLine

Module Program
    Function Main(args As String()) As Integer
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
        Dim p As New Parser(
            Sub(settings)
                settings.HelpWriter = Console.Error
                settings.CaseInsensitiveEnumValues = True
            End Sub)
        Return p.
            ParseArguments(Of LoginVerb, LogoutVerb, StatusVerb, OnlineVerb, DropVerb, DetailVerb)(args).
            MapResult(AddressOf RunVerb, AddressOf RunError)
    End Function

    Private Function RunVerb(opts As VerbBase) As Integer
        Try
            opts.RunAsync().Wait()
            Return 0
        Catch ex As Exception
            Console.Error.WriteLine(ex)
            Return 1
        End Try
    End Function

    Private Function RunError(errs As IEnumerable(Of [Error])) As Integer
        If errs.Any(Function(e) e.Tag = ErrorType.HelpRequestedError OrElse e.Tag = ErrorType.HelpVerbRequestedError OrElse e.Tag = ErrorType.VersionRequestedError) Then
            Return 0
        Else
            Return 1
        End If
    End Function
End Module
