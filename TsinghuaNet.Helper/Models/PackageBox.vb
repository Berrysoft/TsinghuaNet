Public Class PackageBox
    Public Sub New(name As String, license As String)
        Me.Name = name
        Me.License = license
    End Sub

    Public ReadOnly Property Name As String
    Public ReadOnly Property License As String
End Class
