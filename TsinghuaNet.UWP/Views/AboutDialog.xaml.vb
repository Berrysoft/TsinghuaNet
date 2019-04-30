Imports Semver

Public NotInheritable Class AboutDialog
    Inherits ContentDialog

    Public ReadOnly Version As PackageVersion = Package.Current.Id.Version

    Public Function GetVersionString(ver As PackageVersion) As String
        Return $"版本 {ver.Major}.{ver.Minor}.{ver.Build}.{ver.Revision}"
    End Function

    Public ReadOnly Packages As New List(Of PackageBox) From
    {
        New PackageBox("Berrysoft.Tsinghua.Net", "MIT许可证", New SemVersion(1, 1, 224, "rc3")),
        New PackageBox("HtmlAgilityPack", "MIT许可证", New SemVersion(1, 11, 3)),
        New PackageBox("Microsoft.Toolkit.Uwp", "MIT许可证", New SemVersion(5, 1, 1)),
        New PackageBox("Microsoft.Toolkit.Uwp.Connectivity", "MIT许可证", New SemVersion(5, 1, 1)),
        New PackageBox("Microsoft.Toolkit.Uwp.UI.Controls.DataGrid", "MIT许可证", New SemVersion(5, 1, 0)),
        New PackageBox("Microsoft.UI.Xaml", "MIT许可证", New SemVersion(2, 2, 190416008, "prerelease")),
        New PackageBox("Newtonsoft.Json", "MIT许可证", New SemVersion(12, 0, 2)),
        New PackageBox("Refractored.MvvmHelpers", "MIT许可证", New SemVersion(1, 4, 1, "beta")),
        New PackageBox("Semver", "MIT许可证", New SemVersion(2, 0, 5)),
        New PackageBox("WinRTXamlToolkit", "MIT许可证", New SemVersion(2, 3, 0)),
        New PackageBox("WinRTXamlToolkit.Controls.DataVisualization", "MIT许可证", New SemVersion(2, 3, 0))
    }
End Class
