Public NotInheritable Class AboutDialog
    Inherits ContentDialog

    Public ReadOnly Property Version As PackageVersion
        Get
            Return Package.Current.Id.Version
        End Get
    End Property

    Public Function GetVersionString(ver As PackageVersion) As String
        Return $"{ver.Major}.{ver.Minor}.{ver.Build}.{ver.Revision}"
    End Function
End Class
