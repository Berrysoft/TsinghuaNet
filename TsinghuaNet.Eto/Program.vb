Imports Eto
Imports Eto.Forms

Module Program
    Sub Main()
        Dim app As New Application(Platform.Detect)
#If DEBUG Then
        AddHandler app.UnhandledException,
            Sub(sender, e)
                MessageBox.Show("����δ������쳣��" & vbCrLf & e.ExceptionObject.ToString() & vbCrLf & "���򼴽��˳���", "�����쳣", MessageBoxType.Error)
                Application.Instance.Quit()
            End Sub
#End If
        app.Run(New MainForm)
    End Sub
End Module
