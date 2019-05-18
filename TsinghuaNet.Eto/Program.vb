Imports Eto
Imports Eto.Forms

Module Program
    Sub Main()
        Dim app As New Application(Platform.Detect)
#If DEBUG Then
        AddHandler app.UnhandledException,
            Sub(sender, e)
                MessageBox.Show("发生未捕获的异常：" & vbCrLf & e.ExceptionObject.ToString() & vbCrLf & "程序即将退出。", "发生异常", MessageBoxType.Error)
                Application.Instance.Quit()
            End Sub
#End If
        app.Run(New MainForm)
    End Sub
End Module
