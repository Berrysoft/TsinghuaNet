Imports Eto
Imports Eto.Forms

Public Class App
    Inherits Application

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(platformType As String)
        MyBase.New(platformType)
    End Sub

    Public Sub New(platform As Platform)
        MyBase.New(platform)
    End Sub

    Private Sub App_LocalizeString(sender As Object, e As LocalizeEventArgs) Handles Me.LocalizeString
        Select Case e.Text
            Case "&File"
                e.LocalizedText = "文件(&F)"
            Case "&Help"
                e.LocalizedText = "帮助(&H)"
            Case "Quit"
                e.LocalizedText = "退出"
        End Select
    End Sub

#If DEBUG Then
    Private Sub App_UnhandledException(sender As Object, e As UnhandledExceptionEventArgs) Handles Me.UnhandledException
        MessageBox.Show("发生未捕获的异常：" & vbCrLf & e.ExceptionObject.ToString() & vbCrLf & "程序即将退出。", "发生异常", MessageBoxType.Error)
        Quit()
    End Sub
#End If
End Class
