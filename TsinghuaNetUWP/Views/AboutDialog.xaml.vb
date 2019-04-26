Public NotInheritable Class AboutDialog
    Inherits ContentDialog

    Public ReadOnly Property Version As PackageVersion
        Get
            Return Package.Current.Id.Version
        End Get
    End Property

    Public Function GetVersionString(ver As PackageVersion) As String
        Return $"版本 {ver.Major}.{ver.Minor}.{ver.Build}.{ver.Revision}"
    End Function

    Public ReadOnly Packages As New List(Of PackageBox) From
    {
        New PackageBox("Berrysoft.Tsinghua.Net", "MIT许可证"),
        New PackageBox("HtmlAgilityPack", "MIT许可证"),
        New PackageBox("Microsoft.Toolkit.Uwp", "MIT许可证"),
        New PackageBox("Microsoft.UI.Xaml", "MIT许可证"),
        New PackageBox("Refractored.MvvmHelpers", "MIT许可证"),
        New PackageBox("WinRTXamlToolkit", "MIT许可证")
    }
End Class
