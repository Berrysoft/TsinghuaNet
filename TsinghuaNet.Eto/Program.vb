Imports System.ComponentModel
Imports Eto
Imports Eto.Forms

Module Program
    Private WithEvents App As Application

    Sub Main()
        App = New Application(Platform.Detect)
        App.Run(New MainForm)
    End Sub

#If DEBUG Then
    Private Sub App_UnhandledException(sender As Object, e As UnhandledExceptionEventArgs) Handles App.UnhandledException
        MessageBox.Show("发生未捕获的异常：" & vbCrLf & e.ExceptionObject.ToString() & vbCrLf & "程序即将退出。", "发生异常", MessageBoxType.Error)
        App.Quit()
    End Sub
#End If

    Private Sub App_LocalizeString(sender As Object, e As LocalizeEventArgs) Handles App.LocalizeString
        Select Case e.Text
            Case "&File"
                e.LocalizedText = "文件(&F)"
            Case "&Help"
                e.LocalizedText = "帮助(&H)"
            Case "Quit"
                e.LocalizedText = "退出"
        End Select
    End Sub
End Module
