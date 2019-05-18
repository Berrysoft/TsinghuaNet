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
        MessageBox.Show("����δ������쳣��" & vbCrLf & e.ExceptionObject.ToString() & vbCrLf & "���򼴽��˳���", "�����쳣", MessageBoxType.Error)
        App.Quit()
    End Sub
#End If

    Private Sub App_LocalizeString(sender As Object, e As LocalizeEventArgs) Handles App.LocalizeString
        Select Case e.Text
            Case "&File"
                e.LocalizedText = "�ļ�(&F)"
            Case "&Help"
                e.LocalizedText = "����(&H)"
            Case "Quit"
                e.LocalizedText = "�˳�"
        End Select
    End Sub
End Module
