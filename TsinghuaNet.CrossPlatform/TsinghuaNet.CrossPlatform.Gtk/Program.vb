Imports Xamarin.Forms
Imports Xamarin.Forms.Platform.GTK

Module Program
    <STAThread>
    Sub Main()
        Global.Gtk.Application.Init()
        Forms.Init()

        Dim app As New App()
        Dim window As New FormsWindow()
        window.LoadApplication(app)
        window.SetApplicationTitle("�廪��ѧУ԰���ͻ���")
        window.Show()

        Global.Gtk.Application.Run()
    End Sub
End Module
