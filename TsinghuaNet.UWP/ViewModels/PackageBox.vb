Imports Semver

Public Structure PackageBox
    Public Sub New(name As String, license As String, version As SemVersion)
        Me.Name = name
        Me.License = license
        Me.Version = version
    End Sub

    Public ReadOnly Property Name As String
    Public ReadOnly Property License As String
    Public ReadOnly Property Version As SemVersion
End Structure
