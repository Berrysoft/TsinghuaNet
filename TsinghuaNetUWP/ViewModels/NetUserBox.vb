Imports System.Net
Imports Berrysoft.Tsinghua.Net

Public Class NetUserBox
    Inherits DependencyObject
    Implements IEquatable(Of NetUser)

    Public Sub New(Optional user As NetUser = Nothing)
        If user IsNot Nothing Then
            Address = user.Address
            LoginTime = user.LoginTime
            Client = user.Client
        End If
    End Sub

    Public Property Address As IPAddress
    Public Property LoginTime As Date
    Public Property Client As String

    Public Sub Drop()
        RaiseEvent DropUser(Me, Address)
    End Sub

    Public Overloads Function Equals(other As NetUser) As Boolean Implements IEquatable(Of NetUser).Equals
        Return Address.Equals(other.Address) AndAlso LoginTime = other.LoginTime AndAlso Client = other.Client
    End Function

    Public Event DropUser As EventHandler(Of IPAddress)
End Class
