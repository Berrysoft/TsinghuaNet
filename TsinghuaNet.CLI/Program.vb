Imports System.Text
Imports CommandLine

Module Program
    Function Main(args As String()) As Integer
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
        Return Parser.Default.ParseArguments(Of LoginVerb, LogoutVerb, StatusVerb, OnlineVerb, DropVerb, DetailVerb)(args).
            MapResult(
                Function(opts As VerbBase) RunTask(opts.RunAsync()),
                AddressOf RunError)
    End Function

    Private Function RunTask(t As Task) As Integer
        Try
            t.GetAwaiter().GetResult()
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
