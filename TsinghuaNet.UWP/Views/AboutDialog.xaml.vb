Imports TsinghuaNet.Helper

Public NotInheritable Class AboutDialog
    Inherits ContentDialog

    Public ReadOnly Version As PackageVersion = Package.Current.Id.Version

    Public Function GetVersionString(ver As PackageVersion) As String
        Return $"版本 {ver.Major}.{ver.Minor}.{ver.Build}.{ver.Revision}"
    End Function

    Public ReadOnly Packages As New List(Of PackageBox) From
    {
        New PackageBox("HtmlAgilityPack", "MIT许可证"),
        New PackageBox("Microsoft.Toolkit.Uwp", "MIT许可证"),
        New PackageBox("Microsoft.Toolkit.Uwp.Connectivity", "MIT许可证"),
        New PackageBox("Microsoft.Toolkit.Uwp.UI.Controls.DataGrid", "MIT许可证"),
        New PackageBox("Microsoft.UI.Xaml", "MIT许可证"),
        New PackageBox("Newtonsoft.Json", "MIT许可证"),
        New PackageBox("Refractored.MvvmHelpers", "MIT许可证"),
        New PackageBox("WinRTXamlToolkit", "MIT许可证"),
        New PackageBox("WinRTXamlToolkit.Controls.DataVisualization", "MIT许可证")
    }
End Class
